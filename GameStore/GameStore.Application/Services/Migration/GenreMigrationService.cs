using GameStore.Application.Interfaces.Migration;
using GameStore.Data.Entities;
using GameStore.Data.Extensions;
using GameStore.Data.Interfaces;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.Extensions;
using Northwind.Data.Extensions;
using Northwind.Data.Interfaces;

namespace GameStore.Application.Services.Migration;

public class GenreMigrationService : IGenreMigrationService
{
    private readonly IUnitOfWork _coreUnitOfWork;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;

    public GenreMigrationService(IUnitOfWork coreUnitOfWork, IMongoUnitOfWork mongoUnitOfWork)
    {
        _coreUnitOfWork = coreUnitOfWork;
        _mongoUnitOfWork = mongoUnitOfWork;
    }

    public async Task<GenreUpdateDto> MigrateOnUpdateAsync(GenreUpdateDto entity)
    {
        var genre = entity.Genre;
        genre.ParentGenreId = await MigrateParentGenreAsync(genre.ParentGenreId);

        if (genre.Id.IsNotGuidFormat())
        {
            await Task.WhenAll(
                _mongoUnitOfWork.Categories.ThrowIfDoesNotExistWithId(genre.Id),
                _coreUnitOfWork.Genres.ThrowIfGenreNameIsNotUnique(genre.Name));

            var migratedGenre = new Genre
            {
                LegacyId = genre.Id,
                Name = genre.Name,
                ParentGenreId = genre.ParentGenreId.IsGuidFormat() ? Guid.Parse(genre.ParentGenreId!) : null,
            };
            await _coreUnitOfWork.Genres.AddAsync(migratedGenre);
            entity.Genre.Id = migratedGenre.Id.ToString();
        }

        await _coreUnitOfWork.SaveAsync();
        return entity;
    }

    public async Task<GenreCreateDto> MigrateOnCreateAsync(GenreCreateDto entity)
    {
        entity.Genre.ParentGenreId = await MigrateParentGenreAsync(entity.Genre.ParentGenreId);
        await _coreUnitOfWork.SaveAsync();
        return entity;
    }

    private async Task<string?> MigrateParentGenreAsync(string? parentId)
    {
        if (!IsParentMigrationRequired(parentId))
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

    private static bool IsParentMigrationRequired(string? parentId)
    {
        return !string.IsNullOrEmpty(parentId) && parentId.IsNotGuidFormat();
    }
}