using GameStore.Application.Interfaces.Migration;
using GameStore.Application.Interfaces.Util;
using GameStore.Application.Services;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Interfaces.Services;
using Moq;

namespace GameStore.Tests.GameStore.Application.Tests.Services;

public class PublisherFacadeServiceTests
{
    private const string Culture = "en";
    private readonly Mock<IEntityServiceResolver> _mockServiceResolver;
    private readonly Mock<IPublisherMigrationService> _mockMigrationService;
    private readonly PublisherFacadeService _service;

    public PublisherFacadeServiceTests()
    {
        _mockServiceResolver = new Mock<IEntityServiceResolver>();
        _mockMigrationService = new Mock<IPublisherMigrationService>();
        _service = new PublisherFacadeService(_mockServiceResolver.Object, _mockMigrationService.Object);
    }

    [Fact]
    public async Task GetPublisherByIdAsync_GivenId_ReturnsPublisher()
    {
        // Arrange
        var id = Guid.Empty.ToString();
        var expectedPublisher = new PublisherFullDto { Id = id };
        var mockPublisherService = new Mock<IPublisherService>();
        mockPublisherService.Setup(s => s.GetPublisherByIdAsync(id, It.IsAny<string>()))
            .ReturnsAsync(expectedPublisher)
            .Verifiable(Times.Once);
        _mockServiceResolver.Setup(x => x.ResolveForEntityId<IPublisherService>(id))
            .Returns(mockPublisherService.Object)
            .Verifiable(Times.Once);

        // Act
        var result = await _service.GetPublisherByIdAsync(id, Culture);

        // Assert
        Assert.Equal(expectedPublisher.CompanyName, result.CompanyName);
        _mockServiceResolver.Verify();
        mockPublisherService.Verify();
    }

    [Fact]
    public async Task GetAllPublishersAsync_ReturnsAllPublishers()
    {
        // Arrange
        var corePublisherService = new Mock<IPublisherService>();
        var mongoPublisherService = new Mock<IPublisherService>();
        var publishers = new List<PublisherBriefDto> { new() { Id = "123" }, new() { Id = "456" } };

        corePublisherService.Setup(s => s.GetAllPublishersAsync(It.IsAny<string>()))
            .ReturnsAsync(publishers.Take(1).ToList());
        mongoPublisherService.Setup(s => s.GetAllPublishersAsync(It.IsAny<string>()))
            .ReturnsAsync(publishers.TakeLast(1).ToList());
        _mockServiceResolver.Setup(sr => sr.ResolveAll<IPublisherService>())
            .Returns(new[] { corePublisherService.Object, mongoPublisherService.Object });

        // Act
        var result = await _service.GetAllPublishersAsync(Culture);

        // Assert
        Assert.Equal(publishers.Count, result.Count);
        _mockServiceResolver.Verify(x => x.ResolveAll<IPublisherService>(), Times.Once);
    }

    [Fact]
    public async Task GetGamesByPublisherIdAsync_GivenId_ReturnsGames()
    {
        // Arrange
        var id = Guid.Empty.ToString();
        var games = new List<GameBriefDto> { new() { Id = "game1" }, new() { Id = "game2" } };
        var mockPublisherService = new Mock<IPublisherService>();
        mockPublisherService.Setup(s => s.GetGamesByPublisherIdAsync(id, It.IsAny<string>()))
            .ReturnsAsync(games);
        _mockServiceResolver.Setup(sr => sr.ResolveForEntityId<IPublisherService>(id))
            .Returns(mockPublisherService.Object)
            .Verifiable(Times.Once);

        // Act
        var result = await _service.GetGamesByPublisherIdAsync(id, Culture);

        // Assert
        Assert.Equal(games.Count, result.Count);
        _mockServiceResolver.Verify();
    }

    [Fact]
    public async Task AddPublisherAsync_CallsCoreService()
    {
        // Arrange
        var dto = new PublisherCreateDto();

        var mockCoreService = new Mock<ICorePublisherService>();
        mockCoreService.Setup(s => s.AddPublisherAsync(dto))
            .ReturnsAsync(new PublisherBriefDto())
            .Verifiable();

        _mockServiceResolver.Setup(sr => sr.ResolveAll<ICorePublisherService>())
            .Returns(new List<ICorePublisherService> { mockCoreService.Object })
            .Verifiable();

        // Act
        var result = await _service.AddPublisherAsync(dto);

        // Assert
        _mockServiceResolver.Verify(sr => sr.ResolveAll<ICorePublisherService>(), Times.Once);
        mockCoreService.Verify(s => s.AddPublisherAsync(dto), Times.Once);
    }

    [Fact]
    public async Task UpdatePublisherAsync_CallsMigrationAndUpdate()
    {
        // Arrange
        var dto = new PublisherUpdateDto();
        _mockMigrationService.Setup(s => s.MigrateOnUpdateAsync(dto, true))
            .Verifiable();

        var mockCoreService = new Mock<ICorePublisherService>();
        mockCoreService.Setup(s => s.UpdatePublisherAsync(dto))
            .Verifiable();

        _mockServiceResolver.Setup(sr => sr.ResolveAll<ICorePublisherService>())
            .Returns(new List<ICorePublisherService> { mockCoreService.Object })
            .Verifiable();

        // Act
        await _service.UpdatePublisherAsync(dto);

        // Assert
        _mockMigrationService.Verify(s => s.MigrateOnUpdateAsync(dto, true), Times.Once);
        _mockServiceResolver.Verify(sr => sr.ResolveAll<ICorePublisherService>(), Times.Once);
        mockCoreService.Verify(s => s.UpdatePublisherAsync(dto), Times.Once);
    }

    [Fact]
    public async Task DeleteGenreAsync_CallsResolvedService()
    {
        // Arrange
        var mockCoreService = new Mock<IPublisherService>();
        mockCoreService.Setup(s => s.DeletePublisherAsync(It.IsAny<string>()))
            .Verifiable();

        _mockServiceResolver.Setup(sr => sr.ResolveForEntityId<IPublisherService>(It.IsAny<string>()))
            .Returns(mockCoreService.Object)
            .Verifiable();

        // Act
        await _service.DeletePublisherAsync("test-id");

        // Assert
        _mockServiceResolver.Verify(sr => sr.ResolveForEntityId<IPublisherService>(It.IsAny<string>()), Times.Once);
        mockCoreService.Verify(s => s.DeletePublisherAsync(It.IsAny<string>()), Times.Once);
    }
}