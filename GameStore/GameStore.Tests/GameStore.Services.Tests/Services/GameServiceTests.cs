using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Exceptions;
using GameStore.Data.Repositories;
using GameStore.Services.Services;
using GameStore.Shared.DTOs.Game;
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

        _mapper.Setup(m => m.Map<IList<GameBriefDto>>(gamesData))
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
        var game = new Game() { Id = 1, Alias = GameAlias, PlatformId = null, GenreId = null };

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
        var game = new Game() { Alias = GameAlias, PlatformId = null, GenreId = null };

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
        var game = new Game() { Alias = GameAlias, PlatformId = null, GenreId = 0 };

        _mapper.Setup(m => m.Map<Game>(gameCreateDto))
            .Returns(game);

        _unitOfWork.Setup(uow => uow.Games.ExistsAsync(g => g.Alias == GameAlias))
            .ReturnsAsync(false);

        _unitOfWork.Setup(uow => uow.Genres.ExistsAsync(g => g.Id == game.GenreId))
            .ReturnsAsync(false);

        // Act and Assert
        await Assert.ThrowsAsync<ForeignKeyException>(() => _service.AddGameAsync(gameCreateDto));
    }

    [Fact]
    public async Task AddGameAsync_WhenPlatformDoesNotExist_ThrowsForeignKeyException()
    {
        // Arrange
        var gameCreateDto = new GameCreateDto();
        var game = new Game() { Alias = GameAlias, PlatformId = 0, GenreId = null };

        _mapper.Setup(m => m.Map<Game>(gameCreateDto))
            .Returns(game);

        _unitOfWork.Setup(uow => uow.Games.ExistsAsync(g => g.Alias == GameAlias))
            .ReturnsAsync(false);

        _unitOfWork.Setup(uow => uow.Platforms.ExistsAsync(p => p.Id == game.PlatformId))
            .ReturnsAsync(false);

        // Act and Assert
        await Assert.ThrowsAsync<ForeignKeyException>(() => _service.AddGameAsync(gameCreateDto));
    }

    [Fact]
    public async Task UpdateGameAsync_AllOk_CallsRepository()
    {
        // Arrange
        var gameUpdateDto = new GameUpdateDto() { GameId = 1, Alias = GameAlias };
        var existingGame = new Game() { Id = 1, Alias = GameAlias };

        _unitOfWork.Setup(uow => uow.Games.GetByIdAsync(gameUpdateDto.GameId))
            .ReturnsAsync(existingGame);

        _mapper.Setup(m => m.Map(gameUpdateDto, existingGame))
            .Returns(existingGame);

        _unitOfWork.Setup(uow => uow.SaveAsync())
            .ReturnsAsync(1);

        // Act
        await _service.UpdateGameAsync(gameUpdateDto);

        // Assert
        _unitOfWork.Verify(uow => uow.Games.GetByIdAsync(gameUpdateDto.GameId), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateGameAsync_WhenDuplicateAlias_ThrowsEntityAlreadyExistsException()
    {
        // Arrange
        const string updatedAlias = "updated-but-already-exists";
        var gameUpdateDto = new GameUpdateDto() { GameId = 1, Alias = updatedAlias };

        _unitOfWork.Setup(uow => uow.Games.GetByIdAsync(gameUpdateDto.GameId))
            .ReturnsAsync(new Game() { Id = 1, Alias = GameAlias });

        _unitOfWork.Setup(uow => uow.Games.ExistsAsync(g => g.Alias == updatedAlias))
            .ReturnsAsync(true);

        // Act and Assert
        await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => _service.UpdateGameAsync(gameUpdateDto));
    }

    [Fact]
    public async Task UpdateGameAsync_WhenGenreDoesNotExist_ThrowsForeignKeyException()
    {
        // Arrange
        var gameUpdateDto = new GameUpdateDto() { GameId = 1, Alias = GameAlias, GenreId = 3 };
        var existingGame = new Game() { Id = 1, Alias = GameAlias, GenreId = 2 };
        var updatedGame = new Game() { Id = 1, Alias = GameAlias, GenreId = 3 };

        _unitOfWork.Setup(uow => uow.Games.GetByIdAsync(gameUpdateDto.GameId))
            .ReturnsAsync(existingGame);

        _mapper.Setup(m => m.Map(gameUpdateDto, existingGame))
            .Returns(updatedGame);

        _unitOfWork.Setup(uow => uow.Genres.ExistsAsync(g => g.Id == updatedGame.GenreId))
            .ReturnsAsync(false);

        // Act and Assert
        await Assert.ThrowsAsync<ForeignKeyException>(() => _service.UpdateGameAsync(gameUpdateDto));
    }

    [Fact]
    public async Task UpdateGameAsync_WhenPlatformDoesNotExist_ThrowsForeignKeyException()
    {
        // Arrange
        var gameUpdateDto = new GameUpdateDto() { GameId = 1, Alias = GameAlias, PlatformId = 3 };
        var existingGame = new Game() { Id = 1, Alias = GameAlias, PlatformId = 2 };
        var updatedGame = new Game() { Id = 1, Alias = GameAlias, PlatformId = 3 };

        _unitOfWork.Setup(uow => uow.Games.GetByIdAsync(gameUpdateDto.GameId))
            .ReturnsAsync(existingGame);

        _mapper.Setup(m => m.Map(gameUpdateDto, existingGame))
            .Returns(updatedGame);

        _unitOfWork.Setup(uow => uow.Platforms.ExistsAsync(p => p.Id == updatedGame.PlatformId))
            .ReturnsAsync(false);

        // Act and Assert
        await Assert.ThrowsAsync<ForeignKeyException>(() => _service.UpdateGameAsync(gameUpdateDto));
    }

    [Fact]
    public async Task DeleteGameAsync_CallsRepository_WithValidArguments()
    {
        // Arrange
        const long gameId = 1;
        _unitOfWork.Setup(uow => uow.Games.DeleteAsync(gameId)).Returns(Task.CompletedTask);
        _unitOfWork.Setup(uow => uow.SaveAsync()).ReturnsAsync(1);

        // Act
        await _service.DeleteGameAsync(gameId);

        // Assert
        _unitOfWork.Verify(uow => uow.Games.DeleteAsync(gameId), Times.Once);
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