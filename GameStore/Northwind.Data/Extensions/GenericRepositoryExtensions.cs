using System.Diagnostics.CodeAnalysis;
using GameStore.Shared.Exceptions;
using Northwind.Data.Entities;
using Northwind.Data.Interfaces;

namespace Northwind.Data.Extensions;

[ExcludeFromCodeCoverage]
public static class GenericRepositoryExtensions
{
    public static async Task ThrowIfDoesNotExistWithId<T>(this IGenericRepository<T> repository, string id)
        where T : BaseEntity
    {
        if (await repository.ExistsAsync(e => e.Id == id))
        {
            return;
        }

        throw new EntityNotFoundException(entityId: id);
    }
}