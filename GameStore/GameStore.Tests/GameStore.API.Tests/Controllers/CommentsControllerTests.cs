using System.Security.Claims;
using GameStore.API.Controllers;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Comment;
using GameStore.Shared.Exceptions;
using GameStore.Shared.Util;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Controllers;

public class CommentsControllerTests
{
    private readonly Mock<ICommentService> _mockCommentService = new();
    private readonly Mock<IValidatorWrapper<CommentCreateDto>> _mockCreateDtoValidator = new();
    private readonly Mock<IValidatorWrapper<CommentUpdateDto>> _mockUpdateDtoValidator = new();
    private readonly Mock<IAuthorizationService> _mockAuthService = new();
    private readonly CommentsController _controller;

    public CommentsControllerTests()
    {
        _controller = new CommentsController(
            _mockCommentService.Object,
            _mockCreateDtoValidator.Object,
            _mockUpdateDtoValidator.Object,
            _mockAuthService.Object);
    }

    [Fact]
    public async Task GetCommentsByGameAliasAsync_ReturnsListOfComments()
    {
        // Arrange
        _mockCommentService.Setup(s => s.GetCommentsByGameAliasAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<CommentBriefDto> { new(), new() });

        // Act
        var result = await _controller.GetCommentsByGameAliasAsync("gameAlias");

        // Assert
        var list = Assert.IsAssignableFrom<IEnumerable<CommentBriefDto>>(((OkObjectResult)result.Result).Value);
        Assert.Equal(2, list.Count());
    }

    [Fact]
    public void GetBanDurationsAsync_ReturnsBanDurations()
    {
        // Arrange
        _mockCommentService.Setup(service => service.GetBanDurations()).Returns(new List<string> { "1 day", "1 week" });

        // Act
        var result = _controller.GetBanDurationsAsync();

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void BanUser_ReturnsOkResult()
    {
        // Arrange && Act
        var result = _controller.BanUser(new BanUserDto());

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task AddCommentToGameAsync_AllOk_ReturnsListOfComments()
    {
        // Arrange
        const string userName = "test-user";
        const string gameAlias = "gameAlias";
        _mockCommentService.Setup(s => s.AddCommentToGameAsync(It.IsAny<string>(), It.IsAny<CommentCreateDto>()))
            .ReturnsAsync(new List<CommentBriefDto> { new(), new() });
        _mockAuthService.Setup(s =>
            s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Success);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) },
        };
        var commentDto = new CommentCreateDto { Comment = new CommentCreateInnerDto { Name = userName } };

        // Act
        var result = await _controller.AddCommentToGameAsync(gameAlias, commentDto);

        // Assert
        Assert.IsAssignableFrom<IEnumerable<CommentBriefDto>>(((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public async Task AddCommentToGameAsync_UserNameMismatch_ReturnsBadRequest()
    {
        // Arrange
        const string userName = "test-user";
        const string gameAlias = "gameAlias";
        _mockCommentService.Setup(s => s.AddCommentToGameAsync(It.IsAny<string>(), It.IsAny<CommentCreateDto>()))
            .ReturnsAsync(new List<CommentBriefDto> { new(), new() });
        _mockAuthService.Setup(s =>
                s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Success);

        var principal = new ClaimsPrincipal(new ClaimsIdentity(authenticationType: "auth-type", claims: new Claim[]
        {
            new(ClaimTypes.Name, userName),
        }));
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal },
        };
        var commentDto = new CommentCreateDto { Comment = new CommentCreateInnerDto { Name = "wrong user name" } };

        // Act
        var result = await _controller.AddCommentToGameAsync(gameAlias, commentDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task AddCommentToGameAsync_GameFromNorthwind_ThrowsGameStoreNotSupportedException()
    {
        // Arrange
        const string userName = "test-user";
        string gameAlias = EntityAliasUtil.AddSuffix("gameAlias");
        _mockCommentService.Setup(s => s.AddCommentToGameAsync(It.IsAny<string>(), It.IsAny<CommentCreateDto>()))
            .ReturnsAsync(new List<CommentBriefDto> { new(), new() });
        _mockAuthService.Setup(s =>
                s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Success);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) },
        };

        var commentDto = new CommentCreateDto { Comment = new CommentCreateInnerDto { Name = userName } };

        // Act && Assert
        await Assert.ThrowsAsync<GameStoreNotSupportedException>(() => _controller.AddCommentToGameAsync(gameAlias, commentDto));
    }

    [Fact]
    public async Task AddCommentToGameAsync_AuthorizationNotSucceeded_ReturnsForbidden()
    {
        // Arrange
        const string userName = "test-user";
        const string gameAlias = "gameAlias";
        _mockCommentService.Setup(s => s.AddCommentToGameAsync(It.IsAny<string>(), It.IsAny<CommentCreateDto>()))
            .ReturnsAsync(new List<CommentBriefDto> { new(), new() });
        _mockAuthService.Setup(s =>
                s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Failed);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) },
        };
        var commentDto = new CommentCreateDto { Comment = new CommentCreateInnerDto { Name = userName } };

        // Act
        var result = await _controller.AddCommentToGameAsync(gameAlias, commentDto);

        // Assert
        Assert.IsType<ForbidResult>(result.Result);
    }

    [Fact]
    public async Task DeleteCommentAsync_ReturnsNoContent()
    {
        // Arrange && Act
        var result = await _controller.DeleteCommentAsync(Guid.NewGuid());

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}