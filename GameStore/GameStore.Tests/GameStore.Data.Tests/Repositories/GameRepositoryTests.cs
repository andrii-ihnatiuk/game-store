using GameStore.Data;
using GameStore.Data.Entities;
using GameStore.Data.Models;
using GameStore.Data.Repositories;
using GameStore.Shared.Constants.Filter;
using GameStore.Tests.Util;

namespace GameStore.Tests.GameStore.Data.Tests.Repositories;

public class GameRepositoryTests
{
    private readonly GameStoreDbContext _context;
    private readonly GameRepository _repository;
    private readonly GamesFilter _filter;

    public GameRepositoryTests()
    {
        _context = DatabaseService.CreateSqLiteContext();
        _repository = new GameRepository(_context);
        _filter = GetBasicGameFilter();

        ClearDatabase();
        var testData = new List<Game>()
        {
            new Game
            {
                Alias = "Game1",
                Name = "first game",
                Price = 50,
                PublishDate = new DateTime(2021, 1, 1),
                CreationDate = new DateTime(2021, 1, 1),
                Publisher = new Publisher() { CompanyName = "first publisher", Description = string.Empty, HomePage = "homepage" },
                GameGenres = new List<GameGenre>() { new() { Genre = new Genre() { Name = "genre1" } } },
                GamePlatforms = new List<GamePlatform>() { new() { Platform = new Platform() { Type = "type1" } } },
            },
            new Game
            {
                Alias = "Game2",
                Name = "second game",
                Price = 100,
                PublishDate = new DateTime(2022, 1, 1),
                CreationDate = new DateTime(2022, 1, 1),
                Publisher = new Publisher() { CompanyName = "second publisher", Description = string.Empty, HomePage = "homepage" },
                GameGenres = new List<GameGenre>() { new() { Genre = new Genre() { Name = "genre2" } } },
                Comments = new List<Comment>() { new() { Author = "author1", Body = "body1" }, new() { Author = "author2", Body = "body2" } },
            },
            new Game
            {
                Alias = "Game3",
                Name = "third game",
                Price = 150,
                PublishDate = new DateTime(2023, 1, 1),
                CreationDate = new DateTime(2023, 1, 1),
                Comments = new List<Comment>() { new() { Author = "author1", Body = "body1" } },
            },
        };
        _context.Games.AddRange(testData);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetFilteredGamesAsync_WhenGivenMaxPriceFilter_ReturnsOnlyGamesWithLowerOrEqualPrice()
    {
        // Arrange
        var maxPrice = 50;
        _filter.MaxPrice = maxPrice;

        // Act
        var (games, _) = await _repository.GetFilteredGamesAsync(_filter);

        // Assert
        Assert.All(games, game => Assert.True(game.Price <= maxPrice));
    }

    [Fact]
    public async Task GetFilteredGamesAsync_WhenGivenMinPriceFilter_ReturnsOnlyGamesWithHigherOrEqualPrice()
    {
        // Arrange
        var minPrice = 100;
        _filter.MinPrice = minPrice;

        // Act
        var (games, _) = await _repository.GetFilteredGamesAsync(_filter);

        // Assert
        Assert.All(games, game => Assert.True(game.Price >= minPrice));
    }

    [Theory]
    [MemberData(nameof(GetDateFilter))]
    public async Task GetFilteredGamesAsync_WhenGivenDateFilter_ReturnsOnlyGamesWithSameOrLaterPublishDate(string datePublishing, DateTime expectedDate)
    {
        // Arrange
        _filter.DatePublishing = datePublishing;

        // Act
        var (games, _) = await _repository.GetFilteredGamesAsync(_filter);

        // Assert
        Assert.All(games, game => Assert.True(game.PublishDate.HasValue && game.PublishDate >= expectedDate));
    }

    [Fact]
    public async Task GetFilteredGamesAsync_WhenGivenIncorrectDateFilter_ThrowsArgumentOutOfRange()
    {
        // Arrange
        _filter.DatePublishing = "incorrect date filter";

        // Act && Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _repository.GetFilteredGamesAsync(_filter));
    }

    [Fact]
    public async Task GetFilteredGamesAsync_WhenGivenNameFilter_ReturnsOnlyGamesWithName()
    {
        // Arrange
        _filter.Name = "fir";

        // Act
        var (games, _) = await _repository.GetFilteredGamesAsync(_filter);

        // Assert
        Assert.All(games, game => Assert.Contains(_filter.Name, game.Name));
        Assert.NotEmpty(games);
    }

    [Fact]
    public async Task GetFilteredGamesAsync_WhenGivenGenresFilter_ReturnsOnlyGamesOfThatGenres()
    {
        // Arrange
        var genreId = _context.Set<Genre>().Select(g => g.Id).First();
        _filter.Genres = new List<Guid> { genreId };

        // Act
        var (games, _) = await _repository.GetFilteredGamesAsync(_filter);

        // Assert
        Assert.All(games, game => Assert.Contains(game.GameGenres, gg => gg.GenreId == genreId));
    }

    [Fact]
    public async Task GetFilteredGamesAsync_WhenGivenPublishersFilter_ReturnsOnlyGamesOfThatPublishers()
    {
        // Arrange
        var publisherId = _context.Set<Publisher>().Select(p => p.Id).First();
        _filter.Publishers = new List<Guid> { publisherId, };

        // Act
        var (games, _) = await _repository.GetFilteredGamesAsync(_filter);

        // Assert
        Assert.All(games, game => Assert.Equal(publisherId, game.PublisherId));
    }

    [Fact]
    public async Task GetFilteredGamesAsync_WhenGivenPlatformFilter_ReturnsOnlyGamesOfThatPlatform()
    {
        // Arrange
        var platformId = _context.Set<Platform>().Select(p => p.Id).First();
        _filter.Platforms = new List<Guid> { platformId };

        // Act
        var (games, _) = await _repository.GetFilteredGamesAsync(_filter);

        // Assert
        Assert.All(games, game => Assert.Contains(game.GamePlatforms, gp => gp.PlatformId == platformId));
    }

    [Fact]
    public async Task GetFilteredGamesAsync_WhenGivenSortMostCommented_ReturnsGamesSortedByCommentsCount()
    {
        // Arrange
        _filter.Sort = SortingOption.MostCommented;

        // Act
        var (games, _) = await _repository.GetFilteredGamesAsync(_filter);

        // Assert
        Assert.Collection(
            games,
            game1 => Assert.Equal("second game", game1.Name),
            game2 => Assert.Equal("third game", game2.Name),
            game3 => Assert.Equal("first game", game3.Name));
    }

    [Fact]
    public async Task GetFilteredGamesAsync_WhenGivenSortByNewest_ReturnsGamesSortedByCreationDate()
    {
        // Arrange
        _filter.Sort = SortingOption.Newest;

        // Act
        var (games, _) = await _repository.GetFilteredGamesAsync(_filter);

        // Assert
        Assert.Collection(
            games,
            game1 => Assert.Equal("third game", game1.Name),
            game2 => Assert.Equal("second game", game2.Name),
            game3 => Assert.Equal("first game", game3.Name));
    }

    [Fact]
    public async Task GetFilteredGamesAsync_WhenGivenIncorrectSorting_ThrowsArgumentOutOfRange()
    {
        // Arrange
        _filter.Sort = "incorrect sort filter";

        // Act && Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _repository.GetFilteredGamesAsync(_filter));
    }

    private static GamesFilter GetBasicGameFilter()
    {
        return new GamesFilter
        {
            Page = 1,
            PageCount = 10,
        };
    }

    private void ClearDatabase()
    {
        _context.Set<Game>().RemoveRange(_context.Set<Game>());
        _context.Set<Genre>().RemoveRange(_context.Set<Genre>());
        _context.Set<Publisher>().RemoveRange(_context.Set<Publisher>());
        _context.Set<Platform>().RemoveRange(_context.Set<Platform>());
        _context.Set<Comment>().RemoveRange(_context.Set<Comment>());
        _context.SaveChanges();
    }

    private static IEnumerable<object[]> GetDateFilter()
    {
        yield return new object[] { PublishDateOption.LastWeek, DateTime.UtcNow.AddDays(-7) };
        yield return new object[] { PublishDateOption.LastMonth, DateTime.UtcNow.AddMonths(-1) };
        yield return new object[] { PublishDateOption.LastYear, DateTime.UtcNow.AddYears(-1) };
        yield return new object[] { PublishDateOption.LastTwoYears, DateTime.UtcNow.AddYears(-2) };
        yield return new object[] { PublishDateOption.LastThreeYears, DateTime.UtcNow.AddYears(-3) };
    }
}