using AutoMapper;
using GameStore.Application.Interfaces.Migration;
using GameStore.Data.Entities;
using GameStore.Data.Extensions;
using GameStore.Data.Interfaces;
using GameStore.Shared.DTOs.Genre;
using Northwind.Data.Extensions;
using Northwind.Data.Interfaces;

namespace GameStore.Application.Services.Migration;

public class GenreMigrationService : MigrationServiceBase, IGenreMigrationService
{
    private readonly IUnitOfWork _coreUnitOfWork;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly IMapper _mapper;

    public GenreMigrationService(IUnitOfWork coreUnitOfWork, IMongoUnitOfWork mongoUnitOfWork, IMapper mapper)
        : base(coreUnitOfWork)
    {
        _coreUnitOfWork = coreUnitOfWork;
        _mongoUnitOfWork = mongoUnitOfWork;
        _mapper = mapper;
    }

    public async Task<GenreUpdateDto> MigrateOnUpdateAsync(GenreUpdateDto entity, bool commitMigration = true)
    {
        var genre = entity.Genre;
        genre.ParentGenreId = await MigrateParentGenreAsync(genre.ParentGenreId);

        if (IsEntityMigrationRequired(genre.Id))
        {
            await Task.WhenAll(
                _mongoUnitOfWork.Categories.ThrowIfDoesNotExistWithId(genre.Id),
                _coreUnitOfWork.Genres.ThrowIfGenreNameIsNotUnique(genre.Name));

            string legacyId = genre.Id;
            genre.Id = Guid.Empty.ToString();
            var migratedGenre = _mapper.Map<Genre>(entity);
            migratedGenre.LegacyId = legacyId;

            await _coreUnitOfWork.Genres.AddAsync(migratedGenre);
            entity.Genre.Id = migratedGenre.Id.ToString();
        }

        await FinishMigrationAsync(commitMigration);
        return entity;
    }

    public async Task<GenreCreateDto> MigrateOnCreateAsync(GenreCreateDto entity, bool commitMigration = true)
    {
        entity.Genre.ParentGenreId = await MigrateParentGenreAsync(entity.Genre.ParentGenreId);
        await FinishMigrationAsync(commitMigration);
        return entity;
    }

    private async Task<string?> MigrateParentGenreAsync(string? parentId)
    {
        if (IsEntityMigrationNotRequired(parentId))
        {
            return parentId;
        }

        var category = await _mongoUnitOfWork.Categories.GetOneAsync(c => c.Id == parentId);
        await _coreUnitOfWork.Genres.ThrowIfGenreNameIsNotUnique(category.CategoryName);
        var parent = new Genre
        {
            LegacyId = category.Id,
            Name = category.CategoryName,
            Picture = category.Picture,
        };
        await _coreUnitOfWork.Genres.AddAsync(parent);
        return parent.Id.ToString();
    }
}