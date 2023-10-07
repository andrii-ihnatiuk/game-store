using GameStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Data;
public sealed class GameStoreDbContext : DbContext
{
    public GameStoreDbContext(DbContextOptions<GameStoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<Game> Games { get; set; }

    public DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        SeedGames(modelBuilder.Entity<Game>());
        SeedGenres(modelBuilder.Entity<Genre>());
        SeedPlatforms(modelBuilder.Entity<Platform>());
    }

    private static void SeedGames(EntityTypeBuilder<Game> builder)
    {
        builder.HasData(
            new Game()
            {
                Id = 1,
                Alias = "zelda-breath-of-the-wild",
                Name = "The Legend of Zelda: Breath of the Wild",
                Description = "An action-adventure game in an open world.",
                GenreId = 14,
            },
            new Game()
            {
                Id = 2,
                Alias = "gta-v",
                Name = "Grand Theft Auto V",
                Description = "An open-world action-adventure game.",
                GenreId = 11,
            });
    }

    private static void SeedGenres(EntityTypeBuilder<Genre> builder)
    {
        builder.HasData(
            new Genre { Id = 1, Name = "Strategy", ParentGenreId = null },
            new Genre { Id = 2, Name = "RTS", ParentGenreId = 1 },
            new Genre { Id = 3, Name = "TBS", ParentGenreId = 1 },
            new Genre { Id = 4, Name = "RPG", ParentGenreId = null },
            new Genre { Id = 5, Name = "Sports", ParentGenreId = null },
            new Genre { Id = 6, Name = "Races", ParentGenreId = null },
            new Genre { Id = 7, Name = "Rally", ParentGenreId = 6 },
            new Genre { Id = 8, Name = "Arcade", ParentGenreId = 6 },
            new Genre { Id = 9, Name = "Formula", ParentGenreId = 6 },
            new Genre { Id = 10, Name = "Off-road", ParentGenreId = 6 },
            new Genre { Id = 11, Name = "Action", ParentGenreId = null },
            new Genre { Id = 12, Name = "FPS", ParentGenreId = 11 },
            new Genre { Id = 13, Name = "TPS", ParentGenreId = 11 },
            new Genre { Id = 14, Name = "Adventure", ParentGenreId = null },
            new Genre { Id = 15, Name = "Puzzle & Skill", ParentGenreId = null },
            new Genre { Id = 16, Name = "Misc.", ParentGenreId = null });
    }

    private static void SeedPlatforms(EntityTypeBuilder<Platform> builder)
    {
        builder.HasData(
            new Platform { Id = 1, Type = "mobile" },
            new Platform { Id = 2, Type = "browser" },
            new Platform { Id = 3, Type = "desktop" },
            new Platform { Id = 4, Type = "console" });
    }
}
