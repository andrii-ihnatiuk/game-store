using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data;

[ExcludeFromCodeCoverage]
public sealed class GameStoreDbContext : DbContext
{
    public GameStoreDbContext(DbContextOptions<GameStoreDbContext> options)
        : base(options)
    {
    }

    public bool LogChanges { get; set; } = true;

    public DbSet<Game> Games { get; set; }

    public DbSet<Genre> Genres { get; set; }

    public override int SaveChanges()
    {
        SetTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.SeedData();
    }

    private void SetTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e is { State: EntityState.Added, Entity: ICreationTrackable });

        foreach (var entry in entries)
        {
            ((ICreationTrackable)entry.Entity).CreationDate = DateTime.UtcNow;
        }
    }
}
