using System.Security.Claims;
using GameStore.API.Controllers;
using GameStore.Application.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Controllers;

public class PublishersControllerTests
{
    private readonly PublishersController _controller;
    private readonly Mock<IPublisherFacadeService> _publisherFacadeService = new();
    private readonly Mock<IValidatorWrapper<PublisherCreateDto>> _publisherCreateValidator = new();
    private readonly Mock<IValidatorWrapper<PublisherUpdateDto>> _publisherUpdateValidator = new();
    private readonly Mock<IAuthorizationService> _authService = new();

    public PublishersControllerTests()
    {
        _controller = new PublishersController(
            _publisherFacadeService.Object,
            _publisherCreateValidator.Object,
            _publisherUpdateValidator.Object,
            _authService.Object);
    }

    [Fact]
    public async Task GetPublisherAsync_ReturnsPublisher()
    {
        // Arrange
        const string companyName = "company-name";
        _publisherFacadeService.Setup(s => s.GetPublisherByNameAsync(companyName))
            .ReturnsAsync(new PublisherFullDto { CompanyName = companyName }).Verifiable();

        // Act
        var result = await _controller.GetPublisherAsync(companyName);

        // Assert
        _publisherFacadeService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<PublisherFullDto>((result.Result as OkObjectResult).Value);
    }

    [Fact]
    public async Task GetAllPublishersAsync_ReturnsPublishers()
    {
        // Arrange
        _publisherFacadeService.Setup(s => s.GetAllPublishersAsync())
            .ReturnsAsync(new List<PublisherBriefDto>()).Verifiable();

        // Act
        var result = await _controller.GetAllPublishersAsync();

        // Assert
        _publisherFacadeService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IEnumerable<PublisherBriefDto>>((result.Result as OkObjectResult).Value);
    }

    [Fact]
    public async Task GetGamesByPublisherAsync_ReturnsGames()
    {
        // Arrange
        const string publisherName = "publisher-name";
        _publisherFacadeService.Setup(s => s.GetGamesByPublisherNameAsync(publisherName))
            .ReturnsAsync(new List<GameBriefDto>()).Verifiable();

        // Act
        var result = await _controller.GetGamesByPublisherAsync(publisherName);

        // Assert
        _publisherFacadeService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IEnumerable<GameBriefDto>>((result.Result as OkObjectResult).Value);
    }

    [Fact]
    public async Task PostPublisherAsync_ReturnsCreated()
    {
        // Arrange
        var publisherCreateDto = new PublisherCreateDto();
        var publisherBriefDto = new PublisherBriefDto() { CompanyName = "company-name" };
        _publisherFacadeService.Setup(s => s.AddPublisherAsync(publisherCreateDto))
            .ReturnsAsync(publisherBriefDto).Verifiable();

        // Act
        var actionResult = await _controller.PostPublisherAsync(publisherCreateDto);

        // Assert
        _publisherFacadeService.Verify();
        Assert.IsType<CreatedAtRouteResult>(actionResult.Result);
        var routeResult = actionResult.Result as CreatedAtRouteResult;
        Assert.Equal(routeResult.Value, publisherBriefDto);
    }

    [Fact]
    public async Task UpdatePublisherAsync_ReturnsOkResult()
    {
        // Arrange
        var dto = new PublisherUpdateDto();
        _publisherFacadeService.Setup(s => s.UpdatePublisherAsync(dto))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _authService.Setup(s => s.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Success);

        // Act
        var result = await _controller.UpdatePublisherAsync(dto);

        // Assert
        _publisherFacadeService.Verify();
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeletePublisherAsync_ReturnsNoContentResult()
    {
        // Arrange
        _publisherFacadeService.Setup(s => s.DeletePublisherAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _controller.DeletePublisherAsync(Guid.Empty.ToString());

        // Assert
        _publisherFacadeService.Verify();
        Assert.IsType<NoContentResult>(result);
    }
}