using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data;

[ExcludeFromCodeCoverage]
public sealed class GameStoreDbContext : DbContext
{
    public GameStoreDbContext(DbContextOptions<GameStoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<Game> Games { get; set; }

    public DbSet<Genre> Genres { get; set; }

    public override int SaveChanges()
    {
        SetOrderDateTimeProperties();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetOrderDateTimeProperties();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.SeedData();
    }

    private void SetOrderDateTimeProperties()
    {
        var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);

        foreach (var entry in entries)
        {
            switch (entry.Entity)
            {
                case OrderDetail orderDetail:
                    orderDetail.CreationDate = DateTime.UtcNow;
                    break;
                case Order order:
                    order.OrderDate = DateTime.UtcNow;
                    break;
                default:
                    break;
            }
        }
    }
}
