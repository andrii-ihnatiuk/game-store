using AutoMapper;
using GameStore.Application.Interfaces.Migration;
using GameStore.Data.Entities;
using GameStore.Data.Extensions;
using GameStore.Data.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Exceptions;
using Northwind.Data.Extensions;
using Northwind.Data.Interfaces;

namespace GameStore.Application.Services.Migration;

public class GameMigrationService : MigrationServiceBase, IGameMigrationService
{
    private readonly IUnitOfWork _coreUnitOfWork;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly IPublisherMigrationService _publisherMigrationService;
    private readonly IGenreMigrationService _genreMigrationService;
    private readonly IMapper _mapper;

    public GameMigrationService(
        IUnitOfWork coreUnitOfWork,
        IMongoUnitOfWork mongoUnitOfWork,
        IPublisherMigrationService publisherMigrationService,
        IGenreMigrationService genreMigrationService,
        IMapper mapper)
        : base(coreUnitOfWork)
    {
        _coreUnitOfWork = coreUnitOfWork;
        _mongoUnitOfWork = mongoUnitOfWork;
        _publisherMigrationService = publisherMigrationService;
        _genreMigrationService = genreMigrationService;
        _mapper = mapper;
    }

    public async Task<GameUpdateDto> MigrateOnUpdateAsync(GameUpdateDto entity, bool commitMigration = true)
    {
        var game = entity.Game;
        entity.Publisher = await MigrateRelatedPublisherAsync(entity.Publisher);
        entity.Genres = await MigrateRelatedGenresAsync(entity.Genres);

        if (IsEntityMigrationRequired(game.Id))
        {
            await Task.WhenAll(
                _mongoUnitOfWork.Products.ThrowIfDoesNotExistWithId(game.Id),
                _coreUnitOfWork.Games.ThrowIfGameAliasIsNotUnique(game.Key));

            string legacyId = game.Id;
            game.Id = Guid.Empty.ToString();
            var migratedGame = _mapper.Map<Game>(entity);
            migratedGame.LegacyId = legacyId;

            await _coreUnitOfWork.Games.AddAsync(migratedGame);
            game.Id = migratedGame.Id.ToString();
        }

        await FinishMigrationAsync(commitMigration);
        return entity;
    }

    public async Task<GameCreateDto> MigrateOnCreateAsync(GameCreateDto entity, bool commitMigration = true)
    {
        entity.Publisher = await MigrateRelatedPublisherAsync(entity.Publisher);
        entity.Genres = await MigrateRelatedGenresAsync(entity.Genres);
        await FinishMigrationAsync(commitMigration);
        return entity;
    }

    private async Task<string?> MigrateRelatedPublisherAsync(string? publisherId)
    {
        if (IsEntityMigrationNotRequired(publisherId))
        {
            return publisherId;
        }

        var supplier = await _mongoUnitOfWork.Suppliers.GetOneAsync(s => s.Id == publisherId);
        var dto = _mapper.Map<PublisherUpdateDto>(supplier);
        await _publisherMigrationService.MigrateOnUpdateAsync(dto, commitMigration: false);
        return dto.Publisher.Id;
    }

    private async Task<IList<string>> MigrateRelatedGenresAsync(IList<string>? genres)
    {
        var migratedGenres = new List<string>();

        if (genres is null || genres.Count == 0)
        {
            return migratedGenres;
        }

        migratedGenres.AddRange(genres.Where(IsEntityMigrationNotRequired));
        var requiringMigration = genres.Where(g => !migratedGenres.Contains(g));
        var categories = await _mongoUnitOfWork.Categories.GetAllAsync(c => requiringMigration.Contains(c.Id));

        if (requiringMigration.Count() != categories.Count)
        {
            throw new ForeignKeyException(onColumn: nameof(GameUpdateDto.Genres));
        }

        foreach (var category in categories)
        {
            var dto = _mapper.Map<GenreUpdateDto>(category);
            await _genreMigrationService.MigrateOnUpdateAsync(dto, commitMigration: false);
            migratedGenres.Add(dto.Genre.Id);
        }

        return migratedGenres;
    }
}