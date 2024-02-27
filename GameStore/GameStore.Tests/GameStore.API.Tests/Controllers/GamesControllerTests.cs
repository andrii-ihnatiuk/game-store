using System.Security.Claims;
using GameStore.API.Controllers;
using GameStore.Application.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Controllers;

public class GamesControllerTests
{
    private readonly GamesController _controller;
    private readonly Mock<IGameFacadeService> _gameFacadeService = new();
    private readonly Mock<IValidatorWrapper<GameCreateDto>> _gameCreateValidator = new();
    private readonly Mock<IValidatorWrapper<GameUpdateDto>> _gameUpdateValidator = new();
    private readonly Mock<IValidatorWrapper<GamesFilterDto>> _gamesFilterValidator = new();
    private readonly Mock<IAuthorizationService> _mockAuthService = new();

    public GamesControllerTests()
    {
        _controller = new GamesController(
            _gameFacadeService.Object,
            _gameCreateValidator.Object,
            _gameUpdateValidator.Object,
            _gamesFilterValidator.Object,
            _mockAuthService.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext(),
            },
        };

        _controller.HttpContext.Features.Set<IRequestCultureFeature>(
            new RequestCultureFeature(new RequestCulture("en", "en"), null));
    }

    [Fact]
    public async Task GetGameByAliasAsync_ReturnsGameFullDto()
    {
        // Arrange
        const string gameAlias = "game-alias";
        _gameFacadeService.Setup(s => s.GetGameByAliasAsync(gameAlias, It.IsAny<string>()))
            .ReturnsAsync(new GameFullDto { Key = gameAlias })
            .Verifiable();

        // Act
        var result = await _controller.GetGameByAliasAsync(gameAlias);

        // Assert
        _gameFacadeService.Verify();
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<GameFullDto>(((OkObjectResult)result).Value);
    }

    [Fact]
    public async Task GetGameByIdAsync_ReturnsGameFullDto()
    {
        // Arrange
        _gameFacadeService.Setup(s => s.GetGameByIdAsync(Guid.Empty.ToString(), It.IsAny<string>()))
            .ReturnsAsync(new GameFullDto())
            .Verifiable();

        // Act
        var result = await _controller.GetGameByIdAsync(Guid.Empty.ToString());

        // Assert
        _gameFacadeService.Verify();
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<GameFullDto>(((OkObjectResult)result).Value);
    }

    [Fact]
    public async Task GetFilteredGamesAsync_ReturnsGames()
    {
        // Arrange
        _gameFacadeService.Setup(s => s.GetFilteredGamesAsync(
                It.IsAny<GamesFilterDto>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(new FilteredGamesDto(new List<GameFullDto>(), 1, 1))
            .Verifiable();

        var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(CustomClaimTypes.Permission, PermissionOptions.CommentFull),
        }));
        _controller.ControllerContext.HttpContext.User = principal;

        // Act
        var result = await _controller.GetFilteredGamesAsync(new GamesFilterDto());

        // Assert
        _gameFacadeService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<FilteredGamesDto>(((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public async Task GetGenresByGameAliasAsync_ReturnsGenres()
    {
        // Arrange
        string gameAlias = "game-alias";
        _gameFacadeService.Setup(s => s.GetGenresByGameAliasAsync(gameAlias))
            .ReturnsAsync(new List<GenreBriefDto>())
            .Verifiable();

        // Act
        var result = await _controller.GetGenresByGameAliasAsync(gameAlias);

        // Assert
        _gameFacadeService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IEnumerable<GenreBriefDto>>(((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public async Task GetPlatformsByGameAliasAsync_ReturnsPlatforms()
    {
        // Arrange
        string gameAlias = "game-alias";
        _gameFacadeService.Setup(s => s.GetPlatformsByGameAliasAsync(gameAlias))
            .ReturnsAsync(new List<PlatformBriefDto>())
            .Verifiable();

        // Act
        var result = await _controller.GetPlatformsByGameAliasAsync(gameAlias);

        // Assert
        _gameFacadeService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IEnumerable<PlatformBriefDto>>(((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public async Task GetPublisherByGameAliasAsync_ReturnsPublisher()
    {
        // Arrange
        string gameAlias = "game-alias";
        var publisher = new PublisherBriefDto();
        _gameFacadeService.Setup(s => s.GetPublisherByGameAliasAsync(gameAlias))
            .ReturnsAsync(publisher)
            .Verifiable();

        // Act
        var result = await _controller.GetPublisherByGameAliasAsync(gameAlias);

        // Assert
        _gameFacadeService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(publisher, ((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public async Task DownloadGame_ReturnsFile()
    {
        // Arrange
        const string gameAlias = "game-alias";
        _gameFacadeService.Setup(s => s.DownloadAsync(gameAlias))
            .ReturnsAsync(new Tuple<byte[], string>(Array.Empty<byte>(), string.Empty))
            .Verifiable();

        // Act
        var result = await _controller.DownloadGameAsync(gameAlias);

        // Assert
        _gameFacadeService.Verify();
        Assert.IsType<FileContentResult>(result);
    }

    [Fact]
    public async Task PostGame_ReturnsCreated()
    {
        // Arrange
        var gameCreateDto = new GameCreateDto();
        var gameBriefDto = new GameBriefDto() { Key = "game-alias" };
        _gameFacadeService.Setup(s => s.AddGameAsync(gameCreateDto))
            .ReturnsAsync(gameBriefDto)
            .Verifiable();

        // Act
        var result = await _controller.PostGameAsync(gameCreateDto);

        // Assert
        _gameFacadeService.Verify();
        Assert.IsType<CreatedAtRouteResult>(result);
        var routeResult = (CreatedAtRouteResult)result;
        Assert.Equal(routeResult.Value, gameBriefDto);
    }

    [Fact]
    public async Task UpdateGame_ReturnsOk()
    {
        // Arrange
        var dto = new GameUpdateDto();
        _gameFacadeService.Setup(s => s.UpdateGameAsync(dto))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _mockAuthService.Setup(s =>
                s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Success);

        // Act
        var result = await _controller.UpdateGameAsync(dto);

        // Assert
        _gameFacadeService.Verify();
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteGame_ReturnsNoContent()
    {
        // Arrange
        var gameAlias = "game-alias";
        _gameFacadeService.Setup(s => s.DeleteGameAsync(gameAlias))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _controller.DeleteGameAsync(gameAlias);

        // Assert
        _gameFacadeService.Verify();
        Assert.IsType<NoContentResult>(result);
    }
}