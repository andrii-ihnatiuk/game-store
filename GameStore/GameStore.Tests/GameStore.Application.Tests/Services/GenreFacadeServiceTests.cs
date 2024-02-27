using GameStore.Application.Interfaces.Migration;
using GameStore.Application.Interfaces.Util;
using GameStore.Application.Services;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.Interfaces.Services;
using Moq;

namespace GameStore.Tests.GameStore.Application.Tests.Services;

public class GenreFacadeServiceTests
{
    private readonly Mock<IEntityServiceResolver> _mockServiceResolver;
    private readonly Mock<IGenreMigrationService> _mockMigrationService;
    private readonly GenreFacadeService _service;

    public GenreFacadeServiceTests()
    {
        _mockServiceResolver = new Mock<IEntityServiceResolver>();
        _mockMigrationService = new Mock<IGenreMigrationService>();
        _service = new GenreFacadeService(_mockServiceResolver.Object, _mockMigrationService.Object);
    }

    [Fact]
    public async Task GetAllGenresAsync_GivenGenres_ReturnAllGenres()
    {
        // Arrange
        var mockGenreService = new Mock<IGenreService>();
        var genres = new List<GenreBriefDto> { new() { Id = "123" } };
        mockGenreService.Setup(s => s.GetAllGenresAsync(It.IsAny<string>()))
            .ReturnsAsync(genres)
            .Verifiable(Times.Once);
        _mockServiceResolver.Setup(sr => sr.ResolveAll<IGenreService>())
            .Returns(new[] { mockGenreService.Object })
            .Verifiable(Times.Once);

        // Act
        var result = await _service.GetAllGenresAsync(string.Empty);

        // Assert
        Assert.Equal(genres.Count, result.Count);
        _mockServiceResolver.Verify();
        mockGenreService.Verify();
    }

    [Fact]
    public async Task GetGenreByIdAsync_GivenId_ReturnGenre()
    {
        // Arrange
        const string id = "123";
        var genre = new GenreFullDto { Id = id };
        var mockGenreService = new Mock<IGenreService>();
        mockGenreService.Setup(s => s.GetGenreByIdAsync(id, It.IsAny<string>()))
            .ReturnsAsync(genre)
            .Verifiable(Times.Once);
        _mockServiceResolver.Setup(sr => sr.ResolveForEntityId<IGenreService>(id))
            .Returns(mockGenreService.Object)
            .Verifiable(Times.Once);

        // Act
        var result = await _service.GetGenreByIdAsync(id, string.Empty);

        // Assert
        Assert.Equal(genre.Id, result.Id);
        _mockServiceResolver.Verify();
        mockGenreService.Verify();
    }

    [Fact]
    public async Task GetSubgenresByParentAsync_GivenParentId_ReturnsSubgenres()
    {
        // Arrange
        const string parentId = "parentId";
        var genres = new List<GenreBriefDto> { new() { Id = "id1" }, new() { Id = "id2" } };
        var mockGenreService = new Mock<IGenreService>();
        mockGenreService.Setup(s => s.GetSubgenresByParentAsync(parentId, It.IsAny<string>()))
            .ReturnsAsync(genres)
            .Verifiable(Times.Once);
        _mockServiceResolver.Setup(sr => sr.ResolveForEntityId<IGenreService>(parentId))
            .Returns(mockGenreService.Object)
            .Verifiable(Times.Once);

        // Act
        var result = await _service.GetSubgenresByParentAsync(parentId, string.Empty);

        // Assert
        Assert.Equal(genres.Count, result.Count);
        _mockServiceResolver.Verify();
        mockGenreService.Verify();
    }

    [Fact]
    public async Task GetGamesByGenreId_GivenGenreId_ReturnsGames()
    {
        // Arrange
        const string genreId = "genreId";
        var games = new List<GameBriefDto> { new() { Id = "id1" }, new() { Id = "id2" } };
        var mockGenreService = new Mock<IGenreService>();
        mockGenreService.Setup(s => s.GetGamesByGenreIdAsync(genreId, It.IsAny<string>()))
            .ReturnsAsync(games)
            .Verifiable(Times.Once);
        _mockServiceResolver.Setup(sr => sr.ResolveForEntityId<IGenreService>(genreId))
            .Returns(mockGenreService.Object)
            .Verifiable(Times.Once);

        // Act
        var result = await _service.GetGamesByGenreIdAsync(genreId, string.Empty);

        // Assert
        Assert.Equal(games.Count, result.Count);
        _mockServiceResolver.Verify();
        mockGenreService.Verify();
    }

    [Fact]
    public async Task AddGenreAsync_CallsMigrationAndCreate()
    {
        // Arrange
        var dto = new GenreCreateDto();
        _mockMigrationService.Setup(s => s.MigrateOnCreateAsync(dto, true))
            .Verifiable();

        var mockCoreGameService = new Mock<ICoreGenreService>();
        mockCoreGameService.Setup(s => s.AddGenreAsync(dto))
            .ReturnsAsync(new GenreBriefDto())
            .Verifiable();

        _mockServiceResolver.Setup(sr => sr.ResolveAll<ICoreGenreService>())
            .Returns(new List<ICoreGenreService> { mockCoreGameService.Object })
            .Verifiable();

        // Act
        var result = await _service.AddGenreAsync(dto);

        // Assert
        _mockMigrationService.Verify(s => s.MigrateOnCreateAsync(dto, true), Times.Once);
        _mockServiceResolver.Verify(sr => sr.ResolveAll<ICoreGenreService>(), Times.Once);
        mockCoreGameService.Verify(s => s.AddGenreAsync(dto), Times.Once);
    }

    [Fact]
    public async Task UpdateGenreAsync_CallsMigrationAndUpdate()
    {
        // Arrange
        var dto = new GenreUpdateDto();
        _mockMigrationService.Setup(s => s.MigrateOnUpdateAsync(dto, true))
            .Verifiable();

        var mockCoreGameService = new Mock<ICoreGenreService>();
        mockCoreGameService.Setup(s => s.UpdateGenreAsync(dto))
            .Verifiable();

        _mockServiceResolver.Setup(sr => sr.ResolveAll<ICoreGenreService>())
            .Returns(new List<ICoreGenreService> { mockCoreGameService.Object })
            .Verifiable();

        // Act
        await _service.UpdateGenreAsync(dto);

        // Assert
        _mockMigrationService.Verify(s => s.MigrateOnUpdateAsync(dto, true), Times.Once);
        _mockServiceResolver.Verify(sr => sr.ResolveAll<ICoreGenreService>(), Times.Once);
        mockCoreGameService.Verify(s => s.UpdateGenreAsync(dto), Times.Once);
    }

    [Fact]
    public async Task DeleteGenreAsync_CallsResolvedService()
    {
        // Arrange
        var mockCoreGameService = new Mock<ICoreGenreService>();
        mockCoreGameService.Setup(s => s.DeleteGenreAsync(It.IsAny<string>()))
            .Verifiable();

        _mockServiceResolver.Setup(sr => sr.ResolveForEntityId<IGenreService>(It.IsAny<string>()))
            .Returns(mockCoreGameService.Object)
            .Verifiable();

        // Act
        await _service.DeleteGenreAsync("test-id");

        // Assert
        _mockServiceResolver.Verify(sr => sr.ResolveForEntityId<IGenreService>(It.IsAny<string>()), Times.Once);
        mockCoreGameService.Verify(s => s.DeleteGenreAsync(It.IsAny<string>()), Times.Once);
    }
}