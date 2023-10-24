using GameStore.API.Controllers;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Controllers;

public class PublishersControllerTests
{
    private readonly PublishersController _controller;
    private readonly Mock<IPublisherService> _publisherService = new();
    private readonly Mock<IValidatorWrapper<PublisherCreateDto>> _publisherCreateValidator = new();
    private readonly Mock<IValidatorWrapper<PublisherUpdateDto>> _publisherUpdateValidator = new();

    public PublishersControllerTests()
    {
        _controller = new PublishersController(_publisherService.Object, _publisherCreateValidator.Object, _publisherUpdateValidator.Object);
    }

    [Fact]
    public async Task GetPublisherAsync_ReturnsPublisher()
    {
        // Arrange
        const string companyName = "company-name";
        _publisherService.Setup(s => s.GetPublisherByNameAsync(companyName))
            .ReturnsAsync(new PublisherFullDto { CompanyName = companyName }).Verifiable();

        // Act
        var result = await _controller.GetPublisherAsync(companyName);

        // Assert
        _publisherService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<PublisherFullDto>((result.Result as OkObjectResult).Value);
    }

    [Fact]
    public async Task GetAllPublishersAsync_ReturnsPublishers()
    {
        // Arrange
        _publisherService.Setup(s => s.GetAllPublishersAsync())
            .ReturnsAsync(new List<PublisherBriefDto>()).Verifiable();

        // Act
        var result = await _controller.GetAllPublishersAsync();

        // Assert
        _publisherService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IEnumerable<PublisherBriefDto>>((result.Result as OkObjectResult).Value);
    }

    [Fact]
    public async Task GetGamesByPublisherAsync_ReturnsGames()
    {
        // Arrange
        const string publisherName = "publisher-name";
        _publisherService.Setup(s => s.GetGamesByPublisherNameAsync(publisherName))
            .ReturnsAsync(new List<GameBriefDto>()).Verifiable();

        // Act
        var result = await _controller.GetGamesByPublisherAsync(publisherName);

        // Assert
        _publisherService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IEnumerable<GameBriefDto>>((result.Result as OkObjectResult).Value);
    }

    [Fact]
    public async Task PostPublisherAsync_ReturnsCreated()
    {
        // Arrange
        var publisherCreateDto = new PublisherCreateDto();
        var publisherBriefDto = new PublisherBriefDto() { CompanyName = "company-name" };
        _publisherService.Setup(s => s.AddPublisherAsync(publisherCreateDto))
            .ReturnsAsync(publisherBriefDto).Verifiable();

        // Act
        var actionResult = await _controller.PostPublisherAsync(publisherCreateDto);

        // Assert
        _publisherService.Verify();
        Assert.IsType<CreatedAtRouteResult>(actionResult.Result);
        var routeResult = actionResult.Result as CreatedAtRouteResult;
        Assert.Equal(routeResult.Value, publisherBriefDto);
    }

    [Fact]
    public async Task UpdatePublisherAsync_ReturnsOkResult()
    {
        // Arrange
        var dto = new PublisherUpdateDto();
        _publisherService.Setup(s => s.UpdatePublisherAsync(dto))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _controller.UpdatePublisherAsync(dto);

        // Assert
        _publisherService.Verify();
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeletePublisherAsync_ReturnsNoContentResult()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        _publisherService.Setup(s => s.DeletePublisherAsync(publisherId))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _controller.DeletePublisherAsync(publisherId);

        // Assert
        _publisherService.Verify();
        Assert.IsType<NoContentResult>(result);
    }
}