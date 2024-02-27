using AutoFixture;
using AutoMapper;
using GameStore.Application.Interfaces.Migration;
using GameStore.Application.Interfaces.Util;
using GameStore.Application.Services;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Interfaces.Services;
using Moq;

namespace GameStore.Tests.GameStore.Application.Tests.Services;

public class GameFacadeServiceTests
{
    private readonly GameFacadeService _service;
    private readonly Mock<IEntityServiceResolver> _serviceResolverMock = new();
    private readonly Mock<ICoreGameService> _gameServiceMock = new();
    private readonly Mock<IGameMigrationService> _gameMigrationServiceMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Fixture _fixture = new();

    public GameFacadeServiceTests()
    {
        _serviceResolverMock.Setup(sr => sr.ResolveForEntityId<IGameService>(It.IsAny<string>()))
            .Returns(_gameServiceMock.Object);
        _serviceResolverMock.Setup(sr => sr.ResolveForEntityAlias<IGameService>(It.IsAny<string>()))
            .Returns(_gameServiceMock.Object);
        _serviceResolverMock.Setup(sr => sr.ResolveAll<ICoreGameService>())
            .Returns(new List<ICoreGameService> { _gameServiceMock.Object });

        _service = new GameFacadeService(_serviceResolverMock.Object, _gameMigrationServiceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetGameByIdAsync_GivenId_CallsResolverAndService()
    {
        // Arrange
        var gameId = _fixture.Create<string>();
        var expectedGame = _fixture.Create<GameFullDto>();
        expectedGame.Id = gameId;

        _gameServiceMock.Setup(s => s.GetGameByIdAsync(gameId, It.IsAny<string>()))
            .ReturnsAsync(expectedGame)
            .Verifiable(Times.Once);

        // Act
        var result = await _service.GetGameByIdAsync(gameId, string.Empty);

        // Assert
        Assert.Equal(expectedGame.Id, result.Id);
        _gameServiceMock.Verify();
    }

    [Fact]
    public async Task GetGameByAliasAsync_GivenAlias_CallsResolverAndService()
    {
        // Arrange
        var gameAlias = _fixture.Create<string>();
        var expectedGame = _fixture.Create<GameFullDto>();
        expectedGame.Key = gameAlias;

        _gameServiceMock.Setup(s => s.GetGameByAliasAsync(gameAlias, It.IsAny<string>()))
            .ReturnsAsync(expectedGame)
            .Verifiable(Times.Once);

        // Act
        var result = await _service.GetGameByAliasAsync(gameAlias, string.Empty);

        // Assert
        Assert.Equal(expectedGame.Key, result.Key);
        _gameServiceMock.Verify();
    }

    [Fact]
    public async Task GetGenresByGameAliasAsync_GivenAlias_CallsResolverAndService()
    {
        // Arrange
        var alias = _fixture.Create<string>();
        var expectedGenres = _fixture.Create<List<GenreBriefDto>>();

        _gameServiceMock.Setup(s => s.GetGenresByGameAliasAsync(alias))
            .ReturnsAsync(expectedGenres).Verifiable(Times.Once);

        // Act
        var result = await _service.GetGenresByGameAliasAsync(alias);

        // Assert
        Assert.Equal(expectedGenres, result);
        _gameServiceMock.Verify();
    }

    [Fact]
    public async Task GetPlatformsByGameAliasAsync_GivenAlias_CallsResolverAndService()
    {
        // Arrange
        var alias = _fixture.Create<string>();
        var expectedPlatforms = _fixture.Create<List<PlatformBriefDto>>();

        _gameServiceMock.Setup(s => s.GetPlatformsByGameAliasAsync(alias))
            .ReturnsAsync(expectedPlatforms).Verifiable(Times.Once);

        // Act
        var result = await _service.GetPlatformsByGameAliasAsync(alias);

        // Assert
        Assert.Equal(expectedPlatforms, result);
        _gameServiceMock.Verify();
    }

    [Fact]
    public async Task GetPublisherByGameAliasAsync_GivenAlias_CallsResolverAndService()
    {
        // Arrange
        var alias = _fixture.Create<string>();
        var expectedPublisher = _fixture.Create<PublisherBriefDto>();

        _gameServiceMock.Setup(s => s.GetPublisherByGameAliasAsync(alias))
            .ReturnsAsync(expectedPublisher).Verifiable(Times.Once);

        // Act
        var result = await _service.GetPublisherByGameAliasAsync(alias);

        Assert.Equal(expectedPublisher, result);
        _gameServiceMock.Verify();
    }

    [Fact]
    public async Task AddGameAsync_GivenValidDto_CallsMigrationServiceAndAddsGame()
    {
        // Arrange
        var newGame = _fixture.Create<GameCreateDto>();
        var expectedGameDto = _fixture.Create<GameBriefDto>();

        _gameMigrationServiceMock.Setup(s => s.MigrateOnCreateAsync(newGame, true))
            .ReturnsAsync(newGame).Verifiable(Times.Once);
        _gameServiceMock.Setup(s => s.AddGameAsync(newGame))
            .ReturnsAsync(expectedGameDto).Verifiable(Times.Once);

        // Act
        var result = await _service.AddGameAsync(newGame);

        // Assert
        Assert.Equal(expectedGameDto, result);
        _gameMigrationServiceMock.Verify();
        _gameServiceMock.Verify();
    }

    [Fact]
    public async Task UpdateGameAsync_GivenValidDto_CallsMigrationServiceAndUpdatesGame()
    {
        // Arrange
        var gameUpdateDto = _fixture.Create<GameUpdateDto>();

        _gameMigrationServiceMock.Setup(s => s.MigrateOnUpdateAsync(gameUpdateDto, true))
            .ReturnsAsync(gameUpdateDto).Verifiable(Times.Once);
        _gameServiceMock.Setup(s => s.UpdateGameAsync(gameUpdateDto))
            .Returns(Task.CompletedTask).Verifiable(Times.Once);

        // Act
        await _service.UpdateGameAsync(gameUpdateDto);

        // Assert
        _gameMigrationServiceMock.Verify();
        _gameServiceMock.Verify();
    }

    [Fact]
    public async Task DeleteGameAsync_GivenAlias_CallsResolverAndService()
    {
        // Arrange
        var alias = _fixture.Create<string>();
        _gameServiceMock.Setup(s => s.DeleteGameAsync(alias)).Returns(Task.CompletedTask).Verifiable(Times.Once);

        // Act
        await _service.DeleteGameAsync(alias);

        // Assert
        _gameServiceMock.Verify();
    }

    [Fact]
    public async Task DownloadAsync_GivenAlias_CallsResolverAndService()
    {
        // Arrange
        var alias = _fixture.Create<string>();
        var expectedTuple = _fixture.Create<Tuple<byte[], string>>();

        _gameServiceMock.Setup(s => s.DownloadAsync(alias)).ReturnsAsync(expectedTuple).Verifiable(Times.Once);

        var result = await _service.DownloadAsync(alias);

        // Assert
        Assert.Equal(expectedTuple, result);
        _gameServiceMock.Verify();
    }
}