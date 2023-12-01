using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Northwind.Data.Interfaces;

namespace GameStore.Data.Interceptors;

[ExcludeFromCodeCoverage]
public sealed class MongoLoggingInterceptor : SaveChangesInterceptor
{
    private readonly IEntityLogger _logger;

    public MongoLoggingInterceptor(IEntityLogger logger)
    {
        _logger = logger;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            await WriteLogAsync(eventData.Context);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task WriteLogAsync(DbContext context)
    {
        if (context is GameStoreDbContext { LogChanges: false })
        {
            return;
        }

        var trackedEntries = context.ChangeTracker.Entries()
            .Where(s => s.Entity is Game or Genre or Publisher);

        foreach (var entry in trackedEntries)
        {
            var originalObject = entry.OriginalValues.ToObject();
            var currentObject = entry.CurrentValues.ToObject();

            var loggingTask = entry.State switch
            {
                EntityState.Added => _logger.LogCreateAsync(currentObject),
                EntityState.Modified => _logger.LogUpdateAsync(originalObject, currentObject),
                EntityState.Deleted => _logger.LogDeleteAsync(originalObject),
                _ => Task.CompletedTask,
            };
            await loggingTask;
        }
    }
}