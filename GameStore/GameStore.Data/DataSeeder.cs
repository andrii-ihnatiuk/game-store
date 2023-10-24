using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Data;

[ExcludeFromCodeCoverage]
internal static class DataSeeder
{
    private static readonly Guid GameId1 = new("8e9d1000-50e0-4bd8-8159-42c7431f32b5");
    private static readonly Guid GameId2 = new("95ffb14c-267a-432a-9d7c-22f887290d49");

    private static readonly Guid GenreId1 = new("073F790E-A105-491D-965C-946E841C3B3E");
    private static readonly Guid GenreId2 = new("04B04522-E758-4F8C-B58F-F59DD8DE54B5");
    private static readonly Guid GenreId3 = new("EAD8A2BF-E751-47A9-A2FC-E8B6F643B0C8");
    private static readonly Guid GenreId4 = new("5EFE1909-C0B0-440E-8915-2831570816EC");
    private static readonly Guid GenreId5 = new("13BA20D2-42FC-4EAA-A7F9-900782CABFC3");
    private static readonly Guid GenreId6 = new("B3562F18-5E7C-411A-AB76-B675B78BD23D");
    private static readonly Guid GenreId7 = new("2BA0604C-5515-470C-A500-58B60E8BE000");
    private static readonly Guid GenreId8 = new("E0E725ED-50ED-4FF7-94E7-38D0E1D2FA39");
    private static readonly Guid GenreId9 = new("927CC631-C4F1-4FEB-8A0C-9DB6CD43402A");
    private static readonly Guid GenreId10 = new("5D7545F5-A77A-4632-89C6-40F8756D2F75");
    private static readonly Guid GenreId11 = new("E96511B9-E864-44CC-899F-8313609FFB63");
    private static readonly Guid GenreId12 = new("F82E27C8-760E-4CB8-866F-60BB33CAEAAC");
    private static readonly Guid GenreId13 = new("837E2C69-C602-4038-9754-47B9C005A0A8");
    private static readonly Guid GenreId14 = new("DD2BF352-9CFD-4B88-B46F-08217104F90D");
    private static readonly Guid GenreId15 = new("D9EAEF23-9680-4D25-869B-4ED91847FD03");
    private static readonly Guid GenreId16 = new("956C4BAF-E989-4046-B6CB-1D98DF33CF6D");

    private static readonly Guid PlatformId1 = new("a9f806b4-28c5-4d7b-a776-65dfe029de8f");
    private static readonly Guid PlatformId2 = new("467762c6-8a10-4570-b829-e29de78a0757");
    private static readonly Guid PlatformId3 = new("4adb2c38-f819-43cd-aa78-f46d482ceeb7");
    private static readonly Guid PlatformId4 = new("83262eb9-517e-4581-b7ba-88b57c33d721");

    private static readonly Guid PublisherId1 = new("3f8fc430-d0a5-4779-a3a4-5e0add54fde6");
    private static readonly Guid PublisherId2 = new("08029ea0-8bd8-494c-b3c5-b65a61538f81");
    private static readonly Guid PublisherId3 = new("97fd0c5c-0504-4687-9a36-a65e699ca393");

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
                Id = GameId1,
                Alias = "zelda-breath-of-the-wild",
                Name = "The Legend of Zelda: Breath of the Wild",
                Description = "An action-adventure game in an open world.",
                Discontinued = false,
                UnitInStock = 50,
                Price = 1500.2M,
                PublisherId = PublisherId1,
            },
            new Game()
            {
                Id = GameId2,
                Alias = "gta-v",
                Name = "Grand Theft Auto V",
                Description = "An open-world action-adventure game.",
                Discontinued = true,
                UnitInStock = 32,
                Price = 500M,
                PublisherId = PublisherId2,
            });

        builder.Entity<GameGenre>().HasData(
            new GameGenre() { GameId = GameId1, GenreId = GenreId14 },
            new GameGenre() { GameId = GameId1, GenreId = GenreId11 },
            new GameGenre() { GameId = GameId2, GenreId = GenreId11 });

        builder.Entity<GamePlatform>().HasData(
            new GamePlatform() { GameId = GameId1, PlatformId = PlatformId4 },
            new GamePlatform() { GameId = GameId2, PlatformId = PlatformId3 },
            new GamePlatform() { GameId = GameId2, PlatformId = PlatformId4 });
    }

    private static void SeedGenres(EntityTypeBuilder<Genre> builder)
    {
        builder.HasData(
            new Genre { Id = GenreId1, Name = "Strategy", ParentGenreId = null },
            new Genre { Id = GenreId2, Name = "RTS", ParentGenreId = GenreId1 },
            new Genre { Id = GenreId3, Name = "TBS", ParentGenreId = GenreId1 },
            new Genre { Id = GenreId4, Name = "RPG", ParentGenreId = null },
            new Genre { Id = GenreId5, Name = "Sports", ParentGenreId = null },
            new Genre { Id = GenreId6, Name = "Races", ParentGenreId = null },
            new Genre { Id = GenreId7, Name = "Rally", ParentGenreId = GenreId6 },
            new Genre { Id = GenreId8, Name = "Arcade", ParentGenreId = GenreId6 },
            new Genre { Id = GenreId9, Name = "Formula", ParentGenreId = GenreId6 },
            new Genre { Id = GenreId10, Name = "Off-road", ParentGenreId = GenreId6 },
            new Genre { Id = GenreId11, Name = "Action", ParentGenreId = null },
            new Genre { Id = GenreId12, Name = "FPS", ParentGenreId = GenreId11 },
            new Genre { Id = GenreId13, Name = "TPS", ParentGenreId = GenreId11 },
            new Genre { Id = GenreId14, Name = "Adventure", ParentGenreId = null },
            new Genre { Id = GenreId15, Name = "Puzzle & Skill", ParentGenreId = null },
            new Genre { Id = GenreId16, Name = "Misc.", ParentGenreId = null });
    }

    private static void SeedPlatforms(EntityTypeBuilder<Platform> builder)
    {
        builder.HasData(
            new Platform { Id = PlatformId1, Type = "mobile" },
            new Platform { Id = PlatformId2, Type = "browser" },
            new Platform { Id = PlatformId3, Type = "desktop" },
            new Platform { Id = PlatformId4, Type = "console" });
    }

    private static void SeedPublishers(EntityTypeBuilder<Publisher> builder)
    {
        builder.HasData(
            new Publisher() { Id = PublisherId1, CompanyName = "Activision", Description = "Activision Publishing, Inc. is an American video game publisher based in Santa Monica, California.", HomePage = "https://www.activision.com/" },
            new Publisher { Id = PublisherId2, CompanyName = "Electronic Arts", Description = "Electronic Arts Inc. is a global leader in digital interactive entertainment.", HomePage = "https://www.ea.com/" },
            new Publisher { Id = PublisherId3, CompanyName = "Ubisoft", Description = "Ubisoft Entertainment SA is a French video game company headquartered in Montreal.", HomePage = "https://www.ubisoft.com/", });
    }
}