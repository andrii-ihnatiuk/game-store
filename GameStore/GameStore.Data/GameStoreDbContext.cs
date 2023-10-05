using GameStore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data;
public sealed class GameStoreDbContext : DbContext
{
    public GameStoreDbContext(DbContextOptions<GameStoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<Game> Games { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .HasKey(e => e.Alias);
    }
}
