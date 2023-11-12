using GameStore.API.Controllers;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Comment;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Controllers;

public class CommentsControllerTests
{
    [Fact]
    public async Task GetCommentsByGameAliasAsync_ReturnsListOfComments()
    {
        // Arrange
        var mockCommentService = new Mock<ICommentService>();
        mockCommentService.Setup(s => s.GetCommentsByGameAliasAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<CommentBriefDto> { new(), new() });
        var controller = new CommentsController(mockCommentService.Object, Mock.Of<IValidatorWrapper<CommentCreateDto>>());

        // Act
        var result = await controller.GetCommentsByGameAliasAsync("gameAlias");

        // Assert
        var list = Assert.IsAssignableFrom<IEnumerable<CommentBriefDto>>(((OkObjectResult)result.Result).Value);
        Assert.Equal(2, list.Count());
    }

    [Fact]
    public void GetBanDurationsAsync_ReturnsBanDurations()
    {
        // Arrange
        var mockCommentService = new Mock<ICommentService>();
        mockCommentService.Setup(service => service.GetBanDurations()).Returns(new List<string> { "1 day", "1 week" });
        var controller =
            new CommentsController(mockCommentService.Object, Mock.Of<IValidatorWrapper<CommentCreateDto>>());

        // Act
        var result = controller.GetBanDurationsAsync();

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void BanUser_ReturnsOkResult()
    {
        // Arrange
        var mockCommentService = new Mock<ICommentService>();
        var controller =
            new CommentsController(mockCommentService.Object, Mock.Of<IValidatorWrapper<CommentCreateDto>>());

        // Act
        var result = controller.BanUser(new BanUserDto());

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task AddCommentToGameAsync_ReturnsListOfComments()
    {
        // Arrange
        var mockCommentService = new Mock<ICommentService>();
        mockCommentService.Setup(s => s.AddCommentToGameAsync(It.IsAny<string>(), It.IsAny<CommentCreateDto>()))
            .ReturnsAsync(new List<CommentBriefDto> { new(), new() });
        var mockValidator = new Mock<IValidatorWrapper<CommentCreateDto>>();
        mockValidator.Setup(validator => validator.ValidateAndThrow(It.IsAny<CommentCreateDto>()));
        var controller = new CommentsController(mockCommentService.Object, mockValidator.Object);

        // Act
        var result = await controller.AddCommentToGameAsync("gameAlias", new CommentCreateDto());

        // Assert
        Assert.IsAssignableFrom<IEnumerable<CommentBriefDto>>(((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public async Task DeleteCommentAsync_ReturnsNoContent()
    {
        // Arrange
        var mockCommentService = new Mock<ICommentService>();
        var controller =
            new CommentsController(mockCommentService.Object, Mock.Of<IValidatorWrapper<CommentCreateDto>>());

        // Act
        var result = await controller.DeleteCommentAsync(Guid.NewGuid());

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}