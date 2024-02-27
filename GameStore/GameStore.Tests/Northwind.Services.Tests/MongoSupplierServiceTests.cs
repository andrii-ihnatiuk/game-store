using System.Linq.Expressions;
using AutoMapper;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Publisher;
using Moq;
using Northwind.Data.Entities;
using Northwind.Data.Interfaces;
using Northwind.Services;

namespace GameStore.Tests.Northwind.Services.Tests;

public class MongoSupplierServiceTests
{
    private const string Culture = "en";
    private readonly Mock<IMongoUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly MongoSupplierService _service;

    public MongoSupplierServiceTests()
    {
        _mockUnitOfWork = new Mock<IMongoUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _service = new MongoSupplierService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetPublisherByIdAsync_ReturnsPublisherFullDto()
    {
        // Arrange
        var supplier = new Supplier();
        _mockUnitOfWork.Setup(u => u.Suppliers.GetOneAsync(It.IsAny<Expression<Func<Supplier, bool>>>()))
            .ReturnsAsync(supplier);

        var expected = new PublisherFullDto();
        _mockMapper.Setup(m => m.Map<PublisherFullDto>(supplier)).Returns(expected);

        // Act
        var result = await _service.GetPublisherByIdAsync("some-id", Culture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetAllPublishersAsync_ReturnsPublisherBriefDtoList()
    {
        // Arrange
        var suppliers = new List<Supplier> { new() };
        _mockUnitOfWork.Setup(u => u.Suppliers.GetAllAsync(It.IsAny<Expression<Func<Supplier, bool>>>()))
            .ReturnsAsync(suppliers);

        var expected = new List<PublisherBriefDto> { new() };
        _mockMapper.Setup(m => m.Map<IList<PublisherBriefDto>>(suppliers)).Returns(expected);

        // Act
        var result = await _service.GetAllPublishersAsync(Culture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetGamesByPublisherIdAsync_ReturnsGameBriefDtoList()
    {
        // Arrange
        const string id = "some-id";
        var products = new List<Product> { new() };
        _mockUnitOfWork.Setup(u => u.Suppliers.GetProductsBySupplierIdAsync(id)).ReturnsAsync(products);

        var expected = new List<GameBriefDto> { new() };
        _mockMapper.Setup(m => m.Map<IList<GameBriefDto>>(products)).Returns(expected);

        // Act
        var result = await _service.GetGamesByPublisherIdAsync(id, Culture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DeletePublisherAsync_RemovesPublisherAndSavesChanges()
    {
        // Arrange
        const string id = "1";
        _mockUnitOfWork.Setup(u => u.Suppliers.DeleteAsync(id))
            .Verifiable();
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
            .Verifiable();

        // Act
        await _service.DeletePublisherAsync(id);

        // Assert
        _mockUnitOfWork.Verify(u => u.Suppliers.DeleteAsync(id), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}