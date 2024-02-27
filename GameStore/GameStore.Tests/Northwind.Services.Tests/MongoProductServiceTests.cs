using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Models;
using Moq;
using Northwind.Data.Entities;
using Northwind.Data.Interfaces;
using Northwind.Services;

namespace GameStore.Tests.Northwind.Services.Tests;

public class MongoProductServiceTests
{
    private const string DefaultAlias = "alias";
    private readonly Mock<IMongoUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly MongoProductService _service;

    public MongoProductServiceTests()
    {
        _mockUnitOfWork = new Mock<IMongoUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _service = new MongoProductService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetGameByAliasAsync_ReturnsGameFullDto()
    {
        // Arrange
        var product = new Product { Alias = DefaultAlias };
        _mockUnitOfWork.Setup(u => u.Products.GetOneAsync(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(product);

        var expected = new GameFullDto { Key = DefaultAlias };
        _mockMapper.Setup(m => m.Map<GameFullDto>(product)).Returns(expected);

        // Act
        var result = await _service.GetGameByAliasAsync(DefaultAlias, string.Empty);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetGameByIdAsync_ReturnsGameFullDto()
    {
        // Arrange
        const string id = "1";
        var product = new Product { ProductId = long.Parse(id) };
        _mockUnitOfWork.Setup(u => u.Products.GetOneAsync(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(product);

        var expected = new GameFullDto { Id = id };
        _mockMapper.Setup(m => m.Map<GameFullDto>(product)).Returns(expected);

        // Act
        var result = await _service.GetGameByIdAsync(id, string.Empty);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetGenresByGameAliasAsync_ReturnsGenreBriefDtos()
    {
        // Arrange
        var categories = new List<Category> { new(), new() };
        _mockUnitOfWork.Setup(u => u.Products.GetCategoriesByProductAliasAsync(DefaultAlias))
            .ReturnsAsync(categories);

        var expected = new List<GenreBriefDto> { new(), new() };
        _mockMapper.Setup(m => m.Map<IList<GenreBriefDto>>(categories)).Returns(expected);

        // Act
        var result = await _service.GetGenresByGameAliasAsync(DefaultAlias);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(expected.Count, result.Count);
    }

    [Fact]
    public async Task GetPlatformsByGameAliasAsync_ReturnsEmptyPlatformBriefDtos()
    {
        const string alias = "alias";
        var result = await _service.GetPlatformsByGameAliasAsync(alias);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetPublisherByGameAliasAsync_ReturnsPublisherBriefDto()
    {
        // Arrange
        var supplier = new Supplier();
        _mockUnitOfWork.Setup(u => u.Products.GetSupplierByProductAliasAsync(DefaultAlias))
            .ReturnsAsync(supplier);

        var expected = new PublisherBriefDto();
        _mockMapper.Setup(m => m.Map<PublisherBriefDto>(supplier)).Returns(expected);

        // Act
        var result = await _service.GetPublisherByGameAliasAsync(DefaultAlias);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetAllGamesAsync_SupportedFilter_CallsRepository()
    {
        // Arrange
        var filter = new GamesFilter();
        var filteringResult = new EntityFilteringResult<Product>(new List<Product>(), 2);
        _mockUnitOfWork.Setup(u => u.Products.GetFilteredProductsAsync(filter))
            .ReturnsAsync(filteringResult);

        var records = new List<GameFullDto>();
        _mockMapper.Setup(m => m.Map<IList<GameFullDto>>(filteringResult.Records))
            .Returns(records);

        // Act
        var result = await _service.GetFilteredGamesAsync(filter);

        // Assert
        Assert.Equal(2, result.TotalNoLimit);
        Assert.Equal(records, result.Records);
    }

    [Fact]
    public async Task GetAllGamesAsync_NotSupportedFilter_ReturnsEmpty()
    {
        // Arrange
        var filter = new GamesFilter()
        {
            Platforms = new List<Guid>() { Guid.Empty },
            DatePublishing = DateTime.MinValue.ToString(CultureInfo.InvariantCulture),
        };

        // Act
        var result = await _service.GetFilteredGamesAsync(filter);

        // Assert
        Assert.Equal(0, result.TotalNoLimit);
        Assert.Equal(0, result.Records.Count);
    }

    [Fact]
    public async Task DownloadAsync_ReturnsFileContentAndName()
    {
        // Arrange
        var product = new Product { Alias = DefaultAlias, ProductName = "name" };
        _mockUnitOfWork.Setup(u => u.Products.GetOneAsync(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(product);

        // Act
        (byte[] bytes, string fileName) = await _service.DownloadAsync(DefaultAlias);

        // Assert
        var expectedContent = $"Game: {product.ProductName}\n\nDescription:";
        Assert.Equal(Encoding.UTF8.GetBytes(expectedContent), bytes);
        Assert.Contains(product.ProductName, fileName);
    }
}