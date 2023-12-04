using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Shared.Exceptions;

namespace GameStore.Data.Extensions;

[ExcludeFromCodeCoverage]
public static class GenericRepositoryExtensions
{
    public static async Task ThrowIfForeignKeyConstraintViolationFor(this IGenericRepository<Genre> repository, Genre genre)
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
}