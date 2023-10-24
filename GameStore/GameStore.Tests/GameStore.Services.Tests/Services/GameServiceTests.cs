using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Exceptions;
using GameStore.Data.Repositories;
using GameStore.Services.Services;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.DTOs.Publisher;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace GameStore.Tests.GameStore.Services.Tests.Services;

public class GameServiceTests
{
    private const string GameAlias = "test";
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly GameService _service;

    public GameServiceTests()
    {
        _service = new GameService(_unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetGameByAliasAsync_CallsRepository_WithValidArguments()
    {
        // Arrange
        var game = new Game { Alias = GameAlias };
        _unitOfWork.Setup(uow => uow.Games.GetOneAsync(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Func<IQueryable<Game>, IIncludableQueryable<Game, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(game);

        _mapper.Setup(m => m.Map<Game, GameFullDto>(game))
            .Returns(new GameFullDto());

        // Act
        await _service.GetGameByAliasAsync(GameAlias);

        // Assert
        _unitOfWork.Verify(
            uow => uow.Games.GetOneAsync(
                g => g.Alias == GameAlias,
                It.IsAny<Func<IQueryable<Game>, IIncludableQueryable<Game, object>>>(),
                It.IsAny<bool>()),
            Times.Once);
    }

    [Fact]
    public async Task GetGenresByGameAliasAsync_ReturnsGenres()
    {
        // Arrange
        var genresByGame = new List<GameGenre>();
        _unitOfWork.Setup(uow => uow.GamesGenres.GetAsync(
                It.IsAny<Expression<Func<GameGenre, bool>>>(),
                It.IsAny<Func<IQueryable<GameGenre>, IOrderedQueryable<GameGenre>>>(),
                It.IsAny<Func<IQueryable<GameGenre>, IIncludableQueryable<GameGenre, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(genresByGame);
        _mapper.Setup(m => m.Map<IList<GenreBriefDto>>(It.IsAny<object>()))
            .Returns(new List<GenreBriefDto>());

        // Act
        var genres = await _service.GetGenresByGameAliasAsync(GameAlias);

        // Assert
        Assert.NotNull(genres);
        Assert.IsAssignableFrom<IEnumerable<GenreBriefDto>>(genres);
    }

    [Fact]
    public async Task GetPlatformsByGameAliasAsync_ReturnsPlatforms()
    {
        // Arrange
        var platformsByGame = new List<GamePlatform>();
        _unitOfWork.Setup(uow => uow.GamesPlatforms.GetAsync(
                It.IsAny<Expression<Func<GamePlatform, bool>>>(),
                It.IsAny<Func<IQueryable<GamePlatform>, IOrderedQueryable<GamePlatform>>>(),
                It.IsAny<Func<IQueryable<GamePlatform>, IIncludableQueryable<GamePlatform, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(platformsByGame);
        _mapper.Setup(m => m.Map<IList<PlatformBriefDto>>(It.IsAny<object>()))
            .Returns(new List<PlatformBriefDto>());

        // Act
        var platforms = await _service.GetPlatformsByGameAliasAsync(GameAlias);

        // Assert
        Assert.NotNull(platforms);
        Assert.IsAssignableFrom<IEnumerable<PlatformBriefDto>>(platforms);
    }

    [Fact]
    public async Task GetPublisherByGameAliasAsync_ReturnsPublisher()
    {
        // Arrange
        var game = new Game() { Publisher = new Publisher() };
        _unitOfWork.Setup(uow => uow.Games.GetOneAsync(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Func<IQueryable<Game>, IIncludableQueryable<Game, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(game);
        _mapper.Setup(m => m.Map<PublisherBriefDto>(It.IsAny<Publisher>()))
            .Returns(new PublisherBriefDto());

        // Act
        var publisher = await _service.GetPublisherByGameAliasAsync(GameAlias);

        // Assert
        Assert.NotNull(publisher);
        Assert.IsType<PublisherBriefDto>(publisher);
    }

    [Fact]
    public async Task GetAllGamesAsync_ReturnsGames()
    {
        // Arrange
        var gamesData = new List<Game> { new(), new() };
        _unitOfWork.Setup(uow => uow.Games.GetAsync(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                It.IsAny<Func<IQueryable<Game>, IIncludableQueryable<Game, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(gamesData)
            .Verifiable();

        _mapper.Setup(m => m.Map<IEnumerable<GameBriefDto>>(gamesData))
            .Returns(new List<GameBriefDto> { new(), new() });

        // Act
        var games = await _service.GetAllGamesAsync();

        // Assert
        _unitOfWork.Verify();
        Assert.Equal(gamesData.Count, games.Count);
    }

    [Fact]
    public async Task AddGameAsync_AllOk_CallsRepository()
    {
        // Arrange
        var gameCreateDto = new GameCreateDto();
        var game = new Game() { Id = Guid.Empty, Alias = GameAlias };

        _mapper.Setup(m => m.Map<Game>(gameCreateDto))
            .Returns(game);

        _unitOfWork.Setup(uow => uow.Games.ExistsAsync(g => g.Alias == GameAlias))
            .ReturnsAsync(false);

        _unitOfWork.Setup(uow => uow.Games.AddAsync(game))
            .Returns(Task.CompletedTask);

        _unitOfWork.Setup(uow => uow.SaveAsync())
            .ReturnsAsync(1);

        _mapper.Setup(m => m.Map<GameFullDto>(game))
            .Returns(new GameFullDto());

        // Act
        await _service.AddGameAsync(gameCreateDto);

        // Assert
        _unitOfWork.Verify(uow => uow.Games.AddAsync(game), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task AddGameAsync_WhenDuplicateAlias_ThrowsEntityAlreadyExistsException()
    {
        // Arrange
        var gameCreateDto = new GameCreateDto();
        var game = new Game() { Alias = GameAlias };

        _mapper.Setup(m => m.Map<Game>(gameCreateDto))
            .Returns(game);

        _unitOfWork.Setup(uow => uow.Games.ExistsAsync(g => g.Alias == GameAlias))
            .ReturnsAsync(true);

        // Act and Assert
        await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => _service.AddGameAsync(gameCreateDto));
    }

    [Fact]
    public async Task AddGameAsync_WhenGenreDoesNotExist_ThrowsForeignKeyException()
    {
        // Arrange
        var gameCreateDto = new GameCreateDto();
        var gameGenres = new List<GameGenre> { new(), new() };
        var game = new Game() { Alias = GameAlias, GameGenres = gameGenres };

        _mapper.Setup(m => m.Map<Game>(gameCreateDto))
            .Returns(game);

        _unitOfWork.Setup(uow => uow.Games.ExistsAsync(g => g.Alias == GameAlias))
            .ReturnsAsync(false);

        _unitOfWork.Setup(uow => uow.Genres.ExistsAsync(g => g.Id == It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act and Assert
        await Assert.ThrowsAsync<ForeignKeyException>(() => _service.AddGameAsync(gameCreateDto));
    }

    [Fact]
    public async Task AddGameAsync_WhenPlatformDoesNotExist_ThrowsForeignKeyException()
    {
        // Arrange
        var gameCreateDto = new GameCreateDto();
        var gamePlatforms = new List<GamePlatform> { new(), new() };
        var game = new Game() { Alias = GameAlias, GamePlatforms = gamePlatforms };

        _mapper.Setup(m => m.Map<Game>(gameCreateDto))
            .Returns(game);

        _unitOfWork.Setup(uow => uow.Games.ExistsAsync(g => g.Alias == GameAlias))
            .ReturnsAsync(false);

        _unitOfWork.Setup(uow => uow.Platforms.ExistsAsync(p => p.Id == It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act and Assert
        await Assert.ThrowsAsync<ForeignKeyException>(() => _service.AddGameAsync(gameCreateDto));
    }

    [Fact]
    public async Task UpdateGameAsync_AllOk_CallsRepository()
    {
        // Arrange
        var gameUpdateDto = new GameUpdateDto { Game = new GameUpdateInnerDto { Id = Guid.Empty, Key = GameAlias } };
        var existingGame = new Game() { Id = Guid.Empty, Alias = GameAlias };

        _unitOfWork.Setup(uow => uow.Games.GetOneAsync(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Func<IQueryable<Game>, IIncludableQueryable<Game, object?>>?>(),
                It.IsAny<bool>()))
            .ReturnsAsync(existingGame)
            .Verifiable();

        _mapper.Setup(m => m.Map(gameUpdateDto, existingGame))
            .Returns(existingGame);

        _unitOfWork.Setup(uow => uow.SaveAsync())
            .ReturnsAsync(1);

        // Act
        await _service.UpdateGameAsync(gameUpdateDto);

        // Assert
        _unitOfWork.Verify();
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateGameAsync_WhenDuplicateAlias_ThrowsEntityAlreadyExistsException()
    {
        // Arrange
        const string updatedAlias = "updated-but-already-exists";
        var gameUpdateDto = new GameUpdateDto { Game = new GameUpdateInnerDto { Id = Guid.Empty, Key = updatedAlias } };

        _unitOfWork.Setup(uow => uow.Games.GetOneAsync(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Func<IQueryable<Game>, IIncludableQueryable<Game, object?>>?>(),
                It.IsAny<bool>()))
            .ReturnsAsync(new Game() { Id = Guid.Empty, Alias = GameAlias });

        _unitOfWork.Setup(uow => uow.Games.ExistsAsync(g => g.Alias == updatedAlias))
            .ReturnsAsync(true);

        // Act and Assert
        await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => _service.UpdateGameAsync(gameUpdateDto));
    }

    [Fact]
    public async Task UpdateGameAsync_WhenGenreDoesNotExist_ThrowsForeignKeyException()
    {
        // Arrange
        var gameGenres = new List<GameGenre>() { new() };
        var gameUpdateDto = new GameUpdateDto { Game = new GameUpdateInnerDto { Id = Guid.Empty, Key = GameAlias } };

        var existingGame = new Game() { Id = Guid.Empty, Alias = GameAlias };
        _unitOfWork.Setup(uow => uow.Games.GetOneAsync(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Func<IQueryable<Game>, IIncludableQueryable<Game, object?>>?>(),
                It.IsAny<bool>()))
            .ReturnsAsync(existingGame);

        var updatedGame = new Game() { Id = Guid.Empty, Alias = GameAlias, GameGenres = gameGenres };
        _mapper.Setup(m => m.Map(gameUpdateDto, existingGame))
            .Returns(updatedGame);

        // ExistsAsync is called on updatedGame list
        _unitOfWork.Setup(uow => uow.Genres.ExistsAsync(g => g.Id == It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act and Assert
        await Assert.ThrowsAsync<ForeignKeyException>(() => _service.UpdateGameAsync(gameUpdateDto));
    }

    [Fact]
    public async Task UpdateGameAsync_WhenPlatformDoesNotExist_ThrowsForeignKeyException()
    {
        // Arrange
        var gamePlatforms = new List<GamePlatform> { new() };
        var gameUpdateDto = new GameUpdateDto { Game = new GameUpdateInnerDto { Id = Guid.Empty, Key = GameAlias } };

        var existingGame = new Game() { Id = Guid.Empty, Alias = GameAlias };
        _unitOfWork.Setup(uow => uow.Games.GetOneAsync(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Func<IQueryable<Game>, IIncludableQueryable<Game, object?>>?>(),
                It.IsAny<bool>()))
            .ReturnsAsync(existingGame);

        var updatedGame = new Game() { Id = Guid.Empty, Alias = GameAlias, GamePlatforms = gamePlatforms };
        _mapper.Setup(m => m.Map(gameUpdateDto, existingGame))
            .Returns(updatedGame);

        _unitOfWork.Setup(uow => uow.Platforms.ExistsAsync(p => p.Id == It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act and Assert
        await Assert.ThrowsAsync<ForeignKeyException>(() => _service.UpdateGameAsync(gameUpdateDto));
    }

    [Fact]
    public async Task DeleteGameAsync_CallsRepository_WithValidArguments()
    {
        // Arrange
        _unitOfWork.Setup(uow => uow.Games.DeleteAsync(GameAlias)).Returns(Task.CompletedTask);
        _unitOfWork.Setup(uow => uow.SaveAsync()).ReturnsAsync(1);
        _unitOfWork.Setup(uow => uow.Games.GetOneAsync(
                g => g.Alias == GameAlias,
                It.IsAny<Func<IQueryable<Game>, IIncludableQueryable<Game, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(new Game { Id = Guid.Empty });

        // Act
        await _service.DeleteGameAsync(GameAlias);

        // Assert
        _unitOfWork.Verify(uow => uow.Games.DeleteAsync(Guid.Empty), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DownloadAsync_ReturnsWithCorrectResponse()
    {
        // Arrange
        var game = new Game() { Alias = GameAlias, Name = "Test Game", Description = "Test Description" };

        _unitOfWork.Setup(uow => uow.Games.GetOneAsync(
                g => g.Alias == GameAlias,
                It.IsAny<Func<IQueryable<Game>, IIncludableQueryable<Game, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(game);

        // Act
        (byte[] bytes, string fileName) = await _service.DownloadAsync(GameAlias);

        // Assert
        var expectedContent = $"Game: {game.Name}\n\nDescription: {game.Description}";
        Assert.Equal(Encoding.UTF8.GetBytes(expectedContent), bytes);
        Assert.Contains(game.Name, fileName);
    }
}