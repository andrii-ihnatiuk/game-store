using System.Text;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Data.Models;
using GameStore.Services.Interfaces;
using GameStore.Shared.Constants.Filter;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services;

public class CoreGameService : CoreServiceBase, ICoreGameService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CoreGameService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GameFullDto> GetGameByAliasAsync(string alias)
    {
        var game = await _unitOfWork.Games.GetOneAsync(g => g.Alias == alias, noTracking: false);
        game.PageViews += 1;
        await _unitOfWork.SaveAsync(logChanges: false);
        return _mapper.Map<Game, GameFullDto>(game);
    }

    public async Task<GameFullDto> GetGameByIdAsync(string id)
    {
        var gameId = Guid.Parse(id);
        var game = await _unitOfWork.Games.GetOneAsync(g => g.Id == gameId);
        return _mapper.Map<Game, GameFullDto>(game);
    }

    public async Task<IList<GenreBriefDto>> GetGenresByGameAliasAsync(string alias)
    {
        var genresByGame = (await _unitOfWork.GamesGenres.GetAsync(
                predicate: gg => gg.Game.Alias == alias,
                include: q => q.Include(gg => gg.Genre)))
            .Select(gg => gg.Genre);
        return _mapper.Map<IList<GenreBriefDto>>(genresByGame);
    }

    public async Task<IList<PlatformBriefDto>> GetPlatformsByGameAliasAsync(string alias)
    {
        var platformsByGame = (await _unitOfWork.GamesPlatforms.GetAsync(
                predicate: gp => gp.Game.Alias == alias,
                include: q => q.Include(gp => gp.Platform)))
            .Select(gp => gp.Platform);
        return _mapper.Map<IList<PlatformBriefDto>>(platformsByGame);
    }

    public async Task<PublisherBriefDto> GetPublisherByGameAliasAsync(string alias)
    {
        var game = await _unitOfWork.Games.GetOneAsync(
            predicate: g => g.Alias == alias,
            include: q => q.Include(g => g.Publisher));
        return _mapper.Map<PublisherBriefDto>(game.Publisher);
    }

    public async Task<FilteredGamesDto> GetAllGamesAsync(GamesFilterDto filterDto)
    {
        var filter = _mapper.Map<GamesFilter>(filterDto);

        if (filter.Trigger != FilterTrigger.PageChange)
        {
            filter.Page = 1;
        }

        (var filteredGames, int totalPages) = await _unitOfWork.Games.GetFilteredGamesAsync(filter);
        return new FilteredGamesDto()
        {
            Games = _mapper.Map<IList<GameBriefDto>>(filteredGames),
            CurrentPage = filter.Page,
            TotalPages = totalPages,
        };
    }

    public async Task<GameBriefDto> AddGameAsync(GameCreateDto dto)
    {
        var game = _mapper.Map<Game>(dto);
        await ThrowIfGameAliasIsNotUnique(game.Alias);
        await ThrowIfForeignKeyConstraintViolationFor(game);
        await _unitOfWork.Games.AddAsync(game);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<GameBriefDto>(game);
    }

    public async Task UpdateGameAsync(GameUpdateDto dto)
    {
        var existingGame = await _unitOfWork.Games.GetOneAsync(
            predicate: g => g.Id == dto.Game.Id,
            include: q =>
            {
                return q
                    .Include(g => g.GameGenres)
                    .Include(g => g.GamePlatforms);
            },
            noTracking: false);

        if (existingGame.Alias != dto.Game.Key)
        {
            await ThrowIfGameAliasIsNotUnique(dto.Game.Key);
        }

        var updatedGame = _mapper.Map(dto, existingGame);

        await ThrowIfForeignKeyConstraintViolationFor(updatedGame);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteGameAsync(string alias)
    {
        var gameToRemove = await _unitOfWork.Games.GetOneAsync(g => g.Alias == alias);
        await _unitOfWork.Games.DeleteAsync(gameToRemove.Id);
        await _unitOfWork.SaveAsync();
    }

    public async Task<Tuple<byte[], string>> DownloadAsync(string gameAlias)
    {
        var game = await _unitOfWork.Games.GetOneAsync(g => g.Alias == gameAlias);
        string content = $"Game: {game.Name}\n\nDescription: {game.Description}";
        byte[] bytes = Encoding.UTF8.GetBytes(content);
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        string fileName = $"{game.Name}_{timestamp}.txt";
        return new Tuple<byte[], string>(bytes, fileName);
    }

    private async Task ThrowIfForeignKeyConstraintViolationFor(Game game)
    {
        foreach (var gameGenre in game.GameGenres)
        {
            bool genreExists = await _unitOfWork.Genres.ExistsAsync(g => g.Id == gameGenre.GenreId);
            if (!genreExists)
            {
                throw new ForeignKeyException(onColumn: nameof(gameGenre.GenreId));
            }
        }

        foreach (var gamePlatform in game.GamePlatforms)
        {
            bool platformExists = await _unitOfWork.Platforms.ExistsAsync(p => p.Id == gamePlatform.PlatformId);
            if (!platformExists)
            {
                throw new ForeignKeyException(onColumn: nameof(gamePlatform.PlatformId));
            }
        }

        if (game.PublisherId is not null)
        {
            bool publisherExists = await _unitOfWork.Publishers.ExistsAsync(p => p.Id == game.PublisherId);
            if (!publisherExists)
            {
                throw new ForeignKeyException(onColumn: nameof(game.PublisherId));
            }
        }
    }

    private async Task ThrowIfGameAliasIsNotUnique(string alias)
    {
        bool aliasIsNotUnique = await _unitOfWork.Games.ExistsAsync(g => g.Alias == alias);
        if (aliasIsNotUnique)
        {
            throw new EntityAlreadyExistsException(nameof(Game.Alias), alias);
        }
    }
}