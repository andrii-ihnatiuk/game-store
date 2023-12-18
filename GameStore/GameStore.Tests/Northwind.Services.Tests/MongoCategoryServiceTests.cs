using System.Linq.Expressions;
using AutoMapper;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using Moq;
using Northwind.Data.Entities;
using Northwind.Data.Interfaces;
using Northwind.Services;

namespace GameStore.Tests.Northwind.Services.Tests;

public class MongoCategoryServiceTests
{
    private readonly Mock<IMongoUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly MongoCategoryService _service;

    public MongoCategoryServiceTests()
    {
        _mockUnitOfWork = new Mock<IMongoUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _service = new MongoCategoryService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetGenreByIdAsync_ReturnsGenreFullDto()
    {
        const string id = "genreId";
        var genre = new Category { Id = id };
        _mockUnitOfWork.Setup(u => u.Categories.GetOneAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(genre);

        var expected = new GenreFullDto { Id = id };
        _mockMapper.Setup(m => m.Map<GenreFullDto>(genre)).Returns(expected);

        var result = await _service.GetGenreByIdAsync(id);

        Assert.Equal(expected.Id, result.Id);
    }

    [Fact]
    public async Task GetAllGenresAsync_ReturnsGenreBriefDtoList()
    {
        var genres = new List<Category> { new() { Id = "id1" }, new() { Id = "id2" } };
        _mockUnitOfWork.Setup(u => u.Categories.GetAllAsync(null)).ReturnsAsync(genres);

        var expected = new List<GenreBriefDto> { new() { Id = "id1" }, new() { Id = "id2" } };
        _mockMapper.Setup(m => m.Map<IList<GenreBriefDto>>(genres)).Returns(expected);

        var result = await _service.GetAllGenresAsync();

        Assert.Equal(expected.Count, result.Count);
    }

    [Fact]
    public async Task GetSubgenresByParentAsync_ReturnsGenreBriefDtoList()
    {
        const string parentId = "parentId";
        var genres = new List<Category> { new() { ParentId = parentId } };
        _mockUnitOfWork.Setup(u => u.Categories.GetAllAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(genres);

        var expected = new List<GenreBriefDto> { new() { ParentGenreId = parentId } };
        _mockMapper.Setup(m => m.Map<IList<GenreBriefDto>>(genres)).Returns(expected);

        var result = await _service.GetSubgenresByParentAsync(parentId);

        Assert.Equal(expected.Count, result.Count);
    }

    [Fact]
    public async Task GetGamesByGenreIdAsync_ReturnsGameBriefDtoList()
    {
        const string genreId = "genreId";
        var games = new List<Product> { new() { CategoryId = 123 } };
        _mockUnitOfWork.Setup(u => u.Categories.GetProductsByCategoryIdAsync(genreId)).ReturnsAsync(games);

        var expected = new List<GameBriefDto> { new() };
        _mockMapper.Setup(m => m.Map<IList<GameBriefDto>>(games)).Returns(expected);

        var result = await _service.GetGamesByGenreIdAsync(genreId);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DeleteGenreAsync_DeletesGenre()
    {
        const string genreId = "genreId";
        _mockUnitOfWork.Setup(u => u.Categories.DeleteAsync(genreId))
            .Verifiable();
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
            .Verifiable();

        await _service.DeleteGenreAsync(genreId);

        _mockUnitOfWork.Verify(u => u.Categories.DeleteAsync(genreId), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}