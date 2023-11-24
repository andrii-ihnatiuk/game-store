using GameStore.Application.Interfaces;
using GameStore.Application.Services;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.Interfaces.Services;
using Moq;

namespace GameStore.Tests.GameStore.Application.Tests.Services;

public class GenreFacadeServiceTests
{
    private readonly Mock<IServiceResolver> _mockServiceResolver;
    private readonly GenreFacadeService _service;

    public GenreFacadeServiceTests()
    {
        _mockServiceResolver = new Mock<IServiceResolver>();
        _service = new GenreFacadeService(_mockServiceResolver.Object);
    }

    [Fact]
    public async Task GetAllGenresAsync_GivenGenres_ReturnAllGenres()
    {
        // Arrange
        var mockGenreService = new Mock<IGenreService>();
        var genres = new List<GenreBriefDto> { new() { Id = "123" } };
        mockGenreService.Setup(s => s.GetAllGenresAsync()).ReturnsAsync(genres);
        _mockServiceResolver.Setup(sr => sr.ResolveAll<IGenreService>()).Returns(new[] { mockGenreService.Object });

        // Act
        var result = await _service.GetAllGenresAsync();

        // Assert
        Assert.Equal(genres.Count, result.Count);
        _mockServiceResolver.Verify(x => x.ResolveAll<IGenreService>(), Times.Once);
        mockGenreService.Verify(x => x.GetAllGenresAsync(), Times.Once);
    }

    [Fact]
    public async Task GetGenreByIdAsync_GivenId_ReturnGenre()
    {
        // Arrange
        const string id = "123";
        var genre = new GenreFullDto { Id = id };
        var mockGenreService = new Mock<IGenreService>();
        mockGenreService.Setup(s => s.GetGenreByIdAsync(id)).ReturnsAsync(genre);
        _mockServiceResolver.Setup(sr => sr.ResolveForEntityId<IGenreService>(id)).Returns(mockGenreService.Object);

        // Act
        var result = await _service.GetGenreByIdAsync(id);

        // Assert
        Assert.Equal(genre.Id, result.Id);
        _mockServiceResolver.Verify(x => x.ResolveForEntityId<IGenreService>(id), Times.Once);
        mockGenreService.Verify(x => x.GetGenreByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetSubgenresByParentAsync_GivenParentId_ReturnsSubgenres()
    {
        // Arrange
        const string parentId = "parentId";
        var genres = new List<GenreBriefDto> { new() { Id = "id1" }, new() { Id = "id2" } };
        var mockGenreService = new Mock<IGenreService>();
        mockGenreService.Setup(s => s.GetSubgenresByParentAsync(parentId)).ReturnsAsync(genres);
        _mockServiceResolver.Setup(sr => sr.ResolveForEntityId<IGenreService>(parentId)).Returns(mockGenreService.Object);

        // Act
        var result = await _service.GetSubgenresByParentAsync(parentId);

        // Assert
        Assert.Equal(genres.Count, result.Count);
        _mockServiceResolver.Verify(x => x.ResolveForEntityId<IGenreService>(parentId), Times.Once);
        mockGenreService.Verify(x => x.GetSubgenresByParentAsync(parentId), Times.Once);
    }

    [Fact]
    public async Task GetGamesByGenreId_GivenGenreId_ReturnsGames()
    {
        // Arrange
        const string genreId = "genreId";
        var games = new List<GameBriefDto> { new() { Id = "id1" }, new() { Id = "id2" } };
        var mockGenreService = new Mock<IGenreService>();
        mockGenreService.Setup(s => s.GetGamesByGenreId(genreId)).ReturnsAsync(games);
        _mockServiceResolver.Setup(sr => sr.ResolveForEntityId<IGenreService>(genreId)).Returns(mockGenreService.Object);

        // Act
        var result = await _service.GetGamesByGenreId(genreId);

        // Assert
        Assert.Equal(games.Count, result.Count);
        _mockServiceResolver.Verify(x => x.ResolveForEntityId<IGenreService>(genreId), Times.Once);
        mockGenreService.Verify(x => x.GetGamesByGenreId(genreId), Times.Once);
    }
}