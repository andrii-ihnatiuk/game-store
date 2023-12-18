using AutoMapper;
using GameStore.Application.Interfaces;
using GameStore.Application.Interfaces.Migration;
using GameStore.Application.Services;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.Interfaces.Services;
using Moq;

namespace GameStore.Tests.GameStore.Application.Tests.Services;

public class GameFacadeServiceTests
{
    private readonly Mock<IServiceResolver> _mockServiceResolver = new();
    private readonly Mock<IGameMigrationService> _mockMigrationService = new();
    private readonly Mock<IMapper> _mockMapper = new();
    private readonly GameFacadeService _service;

    public GameFacadeServiceTests()
    {
        _service = new GameFacadeService(_mockServiceResolver.Object, _mockMigrationService.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetGameByIdAsync_GivenId_CallsResolverAndService()
    {
        // Arrange
        const string id = "test";
        var expectedGame = new GameFullDto { Id = id };
        var mockGameService = new Mock<IGameService>();
        mockGameService.Setup(s => s.GetGameByIdAsync(id)).ReturnsAsync(expectedGame);
        _mockServiceResolver.Setup(sr => sr.ResolveForEntityId<IGameService>(id)).Returns(mockGameService.Object);

        // Act
        var result = await _service.GetGameByIdAsync(id);

        // Assert
        Assert.Equal(expectedGame.Id, result.Id);
        _mockServiceResolver.Verify(x => x.ResolveForEntityId<IGameService>(id), Times.Once);
        mockGameService.Verify(x => x.GetGameByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetGameByAliasAsync_GivenAlias_CallsResolverAndService()
    {
        // Arrange
        const string alias = "testAlias";
        var expectedGame = new GameFullDto { Key = alias };
        var mockGameService = new Mock<IGameService>();
        mockGameService.Setup(s => s.GetGameByAliasAsync(alias)).ReturnsAsync(expectedGame);
        _mockServiceResolver.Setup(sr => sr.ResolveForEntityAlias<IGameService>(alias)).Returns(mockGameService.Object);

        // Act
        var result = await _service.GetGameByAliasAsync(alias);

        // Assert
        Assert.Equal(expectedGame.Key, result.Key);
        _mockServiceResolver.Verify(x => x.ResolveForEntityAlias<IGameService>(alias), Times.Once);
        mockGameService.Verify(x => x.GetGameByAliasAsync(alias), Times.Once);
    }
}