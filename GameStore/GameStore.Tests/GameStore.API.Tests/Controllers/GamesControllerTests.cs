using GameStore.API.Controllers;
using GameStore.Services.Services;
using GameStore.Shared.DTOs.Game;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Controllers;

public class GamesControllerTests
{
    private readonly GamesController _controller;
    private readonly Mock<IGameService> _gameService = new();

    public GamesControllerTests()
    {
        _controller = new GamesController(_gameService.Object);
    }

    [Fact]
    public async Task GetGameByAlias_ReturnsGameFullDto()
    {
        // Arrange
        const string gameAlias = "game-alias";
        _gameService.Setup(s => s.GetGameByAliasAsync(gameAlias))
            .ReturnsAsync(new GameFullDto { Key = gameAlias })
            .Verifiable();

        // Act
        var result = await _controller.GetGameAsync(gameAlias);

        // Assert
        _gameService.Verify();
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<GameFullDto>(((OkObjectResult)result).Value);
    }

    [Fact]
    public async Task GetAllGames_ReturnsGames()
    {
        // Arrange
        _gameService.Setup(s => s.GetAllGamesAsync())
            .ReturnsAsync(new List<GameBriefDto>())
            .Verifiable();

        // Act
        var result = await _controller.GetAllGamesAsync();

        // Assert
        _gameService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IEnumerable<GameBriefDto>>(((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public async Task DownloadGame_ReturnsFile()
    {
        // Arrange
        const string gameAlias = "game-alias";
        _gameService.Setup(s => s.DownloadAsync(gameAlias))
            .ReturnsAsync(new Tuple<byte[], string>(Array.Empty<byte>(), string.Empty))
            .Verifiable();

        // Act
        var result = await _controller.DownloadGameAsync(gameAlias);

        // Assert
        _gameService.Verify();
        Assert.IsType<FileContentResult>(result);
    }

    [Fact]
    public async Task PostGame_ReturnsCreated()
    {
        // Arrange
        var gameCreateDto = new GameCreateDto();
        var gameFullDto = new GameFullDto() { Key = "game-alias" };
        _gameService.Setup(s => s.AddGameAsync(gameCreateDto))
            .ReturnsAsync(gameFullDto)
            .Verifiable();

        // Act
        var result = await _controller.PostGameAsync(gameCreateDto);

        // Assert
        _gameService.Verify();
        Assert.IsType<CreatedAtRouteResult>(result);
        var routeResult = (CreatedAtRouteResult)result;
        Assert.Equal(routeResult.Value, gameFullDto);
    }

    [Fact]
    public async Task UpdateGame_ReturnsOk()
    {
        // Arrange
        var dto = new GameUpdateDto();
        _gameService.Setup(s => s.UpdateGameAsync(dto))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _controller.UpdateGameAsync(dto);

        // Assert
        _gameService.Verify();
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteGame_ReturnsNoContent()
    {
        // Arrange
        var gameId = Guid.Empty;
        _gameService.Setup(s => s.DeleteGameAsync(gameId))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _controller.DeleteGameAsync(gameId);

        // Assert
        _gameService.Verify();
        Assert.IsType<NoContentResult>(result);
    }
}