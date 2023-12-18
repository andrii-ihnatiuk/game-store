using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Shared.Exceptions;

namespace GameStore.Data.Extensions;

[ExcludeFromCodeCoverage]
public static class GenericRepositoryExtensions
{
    public static async Task ThrowIfForeignKeyConstraintViolation(this IGenericRepository<Genre> repository, Genre genre)
    {
        if (genre.ParentGenreId != null)
        {
            bool parentExists = await repository.ExistsAsync(g => g.Id == genre.ParentGenreId);
            if (!parentExists || genre.Id == genre.ParentGenreId)
            {
                throw new ForeignKeyException(onColumn: nameof(genre.ParentGenreId));
            }
        }
    }

    public static async Task ThrowIfGenreNameIsNotUnique(this IGenericRepository<Genre> repository, string name)
    {
        bool nameIsNotUnique = await repository.ExistsAsync(g => g.Name == name);
        if (nameIsNotUnique)
        {
            throw new EntityAlreadyExistsException(nameof(Genre.Name), name);
        }
    }

    public static async Task ThrowIfForeignKeyConstraintViolation(this IGenericRepository<Genre> repository, IEnumerable<GameGenre> gameGenres)
    {
        foreach (var gameGenre in gameGenres)
        {
            bool genreExists = await repository.ExistsAsync(g => g.Id == gameGenre.GenreId);
            if (!genreExists)
            {
                throw new ForeignKeyException(onColumn: nameof(GameGenre.GenreId));
            }
        }
    }

    public static async Task ThrowIfCompanyNameIsNotUnique(this IGenericRepository<Publisher> repository, string companyName)
    {
        bool nameIsNotUnique = await repository.ExistsAsync(p => p.CompanyName == companyName);
        if (nameIsNotUnique)
        {
            throw new EntityAlreadyExistsException(nameof(Publisher.CompanyName), companyName);
        }
    }

    public static async Task ThrowIfForeignKeyConstraintViolation(this IGenericRepository<Publisher> repository, Game game)
    {
        if (game.PublisherId is not null)
        {
            bool publisherExists = await repository.ExistsAsync(p => p.Id == game.PublisherId);
            if (!publisherExists)
            {
                throw new ForeignKeyException(onColumn: nameof(Game.PublisherId));
            }
        }
    }

    public static async Task ThrowIfForeignKeyConstraintViolation(this IGenericRepository<Platform> repository, IEnumerable<GamePlatform> gamePlatforms)
    {
        foreach (var gamePlatform in gamePlatforms)
        {
            bool platformExists = await repository.ExistsAsync(p => p.Id == gamePlatform.PlatformId);
            if (!platformExists)
            {
                throw new ForeignKeyException(onColumn: nameof(GamePlatform.PlatformId));
            }
        }
    }

    public static async Task ThrowIfGameAliasIsNotUnique(this IGenericRepository<Game> repository, string alias)
    {
        bool aliasIsNotUnique = await repository.ExistsAsync(g => g.Alias == alias);
        if (aliasIsNotUnique)
        {
            throw new EntityAlreadyExistsException(nameof(Game.Alias), alias);
        }
    }
}