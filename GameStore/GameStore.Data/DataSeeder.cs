using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Data;

[ExcludeFromCodeCoverage]
internal static class DataSeeder
{
    private static readonly Guid Id1 = Guid.NewGuid();
    private static readonly Guid Id2 = Guid.NewGuid();
    private static readonly Guid Id3 = Guid.NewGuid();
    private static readonly Guid Id4 = Guid.NewGuid();
    private static readonly Guid Id5 = Guid.NewGuid();
    private static readonly Guid Id6 = Guid.NewGuid();
    private static readonly Guid Id7 = Guid.NewGuid();
    private static readonly Guid Id8 = Guid.NewGuid();
    private static readonly Guid Id9 = Guid.NewGuid();
    private static readonly Guid Id10 = Guid.NewGuid();
    private static readonly Guid Id11 = Guid.NewGuid();
    private static readonly Guid Id12 = Guid.NewGuid();
    private static readonly Guid Id13 = Guid.NewGuid();
    private static readonly Guid Id14 = Guid.NewGuid();
    private static readonly Guid Id15 = Guid.NewGuid();
    private static readonly Guid Id16 = Guid.NewGuid();

    public static void SeedData(this ModelBuilder modelBuilder)
    {
        SeedGenres(modelBuilder.Entity<Genre>());
        SeedPlatforms(modelBuilder.Entity<Platform>());
        SeedPublishers(modelBuilder.Entity<Publisher>());
        SeedGamesContainingGenresAndPlatformsAndPublishers(modelBuilder);
    }

    private static void SeedGamesContainingGenresAndPlatformsAndPublishers(ModelBuilder builder)
    {
        builder.Entity<Game>().HasData(
            new Game()
            {
                Id = Id1,
                Alias = "zelda-breath-of-the-wild",
                Name = "The Legend of Zelda: Breath of the Wild",
                Description = "An action-adventure game in an open world.",
                Discontinued = false,
                UnitInStock = 50,
                Price = 1500.2M,
                PublisherId = Id1,
            },
            new Game()
            {
                Id = Id2,
                Alias = "gta-v",
                Name = "Grand Theft Auto V",
                Description = "An open-world action-adventure game.",
                Discontinued = true,
                UnitInStock = 32,
                Price = 500M,
                PublisherId = Id2,
            });

        builder.Entity<GameGenre>().HasData(
            new GameGenre() { GameId = Id1, GenreId = Id14 },
            new GameGenre() { GameId = Id1, GenreId = Id11 },
            new GameGenre() { GameId = Id2, GenreId = Id11 });

        builder.Entity<GamePlatform>().HasData(
            new GamePlatform() { GameId = Id1, PlatformId = Id4 },
            new GamePlatform() { GameId = Id2, PlatformId = Id3 },
            new GamePlatform() { GameId = Id2, PlatformId = Id4 });
    }

    private static void SeedGenres(EntityTypeBuilder<Genre> builder)
    {
        builder.HasData(
            new Genre { Id = Id1, Name = "Strategy", ParentGenreId = null },
            new Genre { Id = Id2, Name = "RTS", ParentGenreId = Id1 },
            new Genre { Id = Id3, Name = "TBS", ParentGenreId = Id1 },
            new Genre { Id = Id4, Name = "RPG", ParentGenreId = null },
            new Genre { Id = Id5, Name = "Sports", ParentGenreId = null },
            new Genre { Id = Id6, Name = "Races", ParentGenreId = null },
            new Genre { Id = Id7, Name = "Rally", ParentGenreId = Id6 },
            new Genre { Id = Id8, Name = "Arcade", ParentGenreId = Id6 },
            new Genre { Id = Id9, Name = "Formula", ParentGenreId = Id6 },
            new Genre { Id = Id10, Name = "Off-road", ParentGenreId = Id6 },
            new Genre { Id = Id11, Name = "Action", ParentGenreId = null },
            new Genre { Id = Id12, Name = "FPS", ParentGenreId = Id11 },
            new Genre { Id = Id13, Name = "TPS", ParentGenreId = Id11 },
            new Genre { Id = Id14, Name = "Adventure", ParentGenreId = null },
            new Genre { Id = Id15, Name = "Puzzle & Skill", ParentGenreId = null },
            new Genre { Id = Id16, Name = "Misc.", ParentGenreId = null });
    }

    private static void SeedPlatforms(EntityTypeBuilder<Platform> builder)
    {
        builder.HasData(
            new Platform { Id = Id1, Type = "mobile" },
            new Platform { Id = Id2, Type = "browser" },
            new Platform { Id = Id3, Type = "desktop" },
            new Platform { Id = Id4, Type = "console" });
    }

    private static void SeedPublishers(EntityTypeBuilder<Publisher> builder)
    {
        builder.HasData(
            new Publisher() { Id = Id1, CompanyName = "Activision", Description = "Activision Publishing, Inc. is an American video game publisher based in Santa Monica, California.", HomePage = "https://www.activision.com/" },
            new Publisher { Id = Id2, CompanyName = "Electronic Arts", Description = "Electronic Arts Inc. is a global leader in digital interactive entertainment.", HomePage = "https://www.ea.com/" },
            new Publisher { Id = Id3, CompanyName = "Ubisoft", Description = "Ubisoft Entertainment SA is a French video game company headquartered in Montreal.", HomePage = "https://www.ubisoft.com/", });
    }
}