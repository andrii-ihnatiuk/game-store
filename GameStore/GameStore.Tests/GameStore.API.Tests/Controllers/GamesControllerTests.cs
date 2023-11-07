using GameStore.API.Controllers;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Controllers;

public class GamesControllerTests
{
    private readonly GamesController _controller;
    private readonly Mock<IGameService> _gameService = new();
    private readonly Mock<IValidatorWrapper<GameCreateDto>> _gameCreateValidator = new();
    private readonly Mock<IValidatorWrapper<GameUpdateDto>> _gameUpdateValidator = new();
    private readonly Mock<IValidatorWrapper<GamesFilterDto>> _gamesFilterValidator = new();

    public GamesControllerTests()
    {
        _controller = new GamesController(_gameService.Object, _gameCreateValidator.Object, _gameUpdateValidator.Object, _gamesFilterValidator.Object);
    }

    [Fact]
    public async Task GetGameByAliasAsync_ReturnsGameFullDto()
    {
        // Arrange
        const string gameAlias = "game-alias";
        _gameService.Setup(s => s.GetGameByAliasAsync(gameAlias))
            .ReturnsAsync(new GameFullDto { Key = gameAlias })
            .Verifiable();

        // Act
        var result = await _controller.GetGameByAliasAsync(gameAlias);

        // Assert
        _gameService.Verify();
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<GameFullDto>(((OkObjectResult)result).Value);
    }

    [Fact]
    public async Task GetGameByIdAsync_ReturnsGameFullDto()
    {
        // Arrange
        _gameService.Setup(s => s.GetGameByIdAsync(Guid.Empty))
            .ReturnsAsync(new GameFullDto())
            .Verifiable();

        // Act
        var result = await _controller.GetGameByIdAsync(Guid.Empty);

        // Assert
        _gameService.Verify();
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<GameFullDto>(((OkObjectResult)result).Value);
    }

    [Fact]
    public async Task GetAllGamesAsync_ReturnsGames()
    {
        // Arrange
        _gameService.Setup(s => s.GetAllGamesAsync(It.IsAny<GamesFilterDto>()))
            .ReturnsAsync(new FilteredGamesDto())
            .Verifiable();

        // Act
        var result = await _controller.GetAllGamesAsync(new GamesFilterDto());

        // Assert
        _gameService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<FilteredGamesDto>(((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public async Task GetGenresByGameAliasAsync_ReturnsGenres()
    {
        // Arrange
        string gameAlias = "game-alias";
        _gameService.Setup(s => s.GetGenresByGameAliasAsync(gameAlias))
            .ReturnsAsync(new List<GenreBriefDto>())
            .Verifiable();

        // Act
        var result = await _controller.GetGenresByGameAliasAsync(gameAlias);

        // Assert
        _gameService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IEnumerable<GenreBriefDto>>(((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public async Task GetPlatformsByGameAliasAsync_ReturnsPlatforms()
    {
        // Arrange
        string gameAlias = "game-alias";
        _gameService.Setup(s => s.GetPlatformsByGameAliasAsync(gameAlias))
            .ReturnsAsync(new List<PlatformBriefDto>())
            .Verifiable();

        // Act
        var result = await _controller.GetPlatformsByGameAliasAsync(gameAlias);

        // Assert
        _gameService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IEnumerable<PlatformBriefDto>>(((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public async Task GetPublisherByGameAliasAsync_ReturnsPublisher()
    {
        // Arrange
        string gameAlias = "game-alias";
        var publisher = new PublisherBriefDto();
        _gameService.Setup(s => s.GetPublisherByGameAliasAsync(gameAlias))
            .ReturnsAsync(publisher)
            .Verifiable();

        // Act
        var result = await _controller.GetPublisherByGameAliasAsync(gameAlias);

        // Assert
        _gameService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(publisher, ((OkObjectResult)result.Result).Value);
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
        var gameBriefDto = new GameBriefDto() { Key = "game-alias" };
        _gameService.Setup(s => s.AddGameAsync(gameCreateDto))
            .ReturnsAsync(gameBriefDto)
            .Verifiable();

        // Act
        var result = await _controller.PostGameAsync(gameCreateDto);

        // Assert
        _gameService.Verify();
        Assert.IsType<CreatedAtRouteResult>(result);
        var routeResult = (CreatedAtRouteResult)result;
        Assert.Equal(routeResult.Value, gameBriefDto);
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
        var gameAlias = "game-alias";
        _gameService.Setup(s => s.DeleteGameAsync(gameAlias))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _controller.DeleteGameAsync(gameAlias);

        // Assert
        _gameService.Verify();
        Assert.IsType<NoContentResult>(result);
    }
}