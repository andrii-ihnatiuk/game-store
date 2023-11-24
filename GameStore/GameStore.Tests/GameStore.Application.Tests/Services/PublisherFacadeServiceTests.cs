using GameStore.Application.Interfaces;
using GameStore.Application.Services;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Interfaces.Services;
using Moq;

namespace GameStore.Tests.GameStore.Application.Tests.Services;

public class PublisherFacadeServiceTests
{
    private readonly Mock<IServiceResolver> _mockServiceResolver;
    private readonly PublisherFacadeService _service;

    public PublisherFacadeServiceTests()
    {
        _mockServiceResolver = new Mock<IServiceResolver>();
        _service = new PublisherFacadeService(_mockServiceResolver.Object);
    }

    [Fact]
    public async Task GetPublisherByNameAsync_GivenName_ReturnsPublisher()
    {
        // Arrange
        const string name = "PublisherName";
        var expectedPublisher = new PublisherFullDto { CompanyName = name };
        var mockPublisherService = new Mock<IPublisherService>();
        mockPublisherService.Setup(s => s.GetPublisherByNameAsync(name)).ReturnsAsync(expectedPublisher);
        _mockServiceResolver.Setup(x => x.ResolveForEntityAlias<IPublisherService>(name))
            .Returns(mockPublisherService.Object);

        // Act
        var result = await _service.GetPublisherByNameAsync(name);

        // Assert
        Assert.Equal(expectedPublisher.CompanyName, result.CompanyName);
        _mockServiceResolver.Verify(x => x.ResolveForEntityAlias<IPublisherService>(name), Times.Once);
        mockPublisherService.Verify(x => x.GetPublisherByNameAsync(name), Times.Once);
    }

    [Fact]
    public async Task GetAllPublishersAsync_ReturnsAllPublishers()
    {
        // Arrange
        var corePublisherService = new Mock<IPublisherService>();
        var mongoPublisherService = new Mock<IPublisherService>();
        var publishers = new List<PublisherBriefDto> { new() { Id = "123" }, new() { Id = "456" } };

        corePublisherService.Setup(s => s.GetAllPublishersAsync()).ReturnsAsync(publishers.Take(1).ToList());
        mongoPublisherService.Setup(s => s.GetAllPublishersAsync()).ReturnsAsync(publishers.TakeLast(1).ToList());
        _mockServiceResolver.Setup(sr => sr.ResolveAll<IPublisherService>())
            .Returns(new[] { corePublisherService.Object, mongoPublisherService.Object });

        // Act
        var result = await _service.GetAllPublishersAsync();

        // Assert
        Assert.Equal(publishers.Count, result.Count);
        _mockServiceResolver.Verify(x => x.ResolveAll<IPublisherService>(), Times.Once);
    }

    [Fact]
    public async Task GetGamesByPublisherNameAsync_GivenName_ReturnsGames()
    {
        // Arrange
        const string name = "PublisherName";
        var games = new List<GameBriefDto> { new() { Id = "game1" }, new() { Id = "game2" } };
        var mockPublisherService = new Mock<IPublisherService>();
        mockPublisherService.Setup(s => s.GetGamesByPublisherNameAsync(name)).ReturnsAsync(games);
        _mockServiceResolver.Setup(sr => sr.ResolveForEntityAlias<IPublisherService>(name))
            .Returns(mockPublisherService.Object);

        // Act
        var result = await _service.GetGamesByPublisherNameAsync(name);

        // Assert
        Assert.Equal(games.Count, result.Count);
        _mockServiceResolver.Verify(x => x.ResolveForEntityAlias<IPublisherService>(name), Times.Once);
    }
}