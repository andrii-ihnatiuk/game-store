using System.Linq.Expressions;
using AutoMapper;
using GameStore.Application.Services.Migration;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Shared.DTOs.Publisher;
using Moq;
using Northwind.Data.Entities;
using Northwind.Data.Interfaces;

namespace GameStore.Tests.GameStore.Application.Tests.MigrationServices;

public class PublisherMigrationServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMongoUnitOfWork> _mockMongoUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly PublisherMigrationService _service;

    public PublisherMigrationServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMongoUnitOfWork = new Mock<IMongoUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _service = new PublisherMigrationService(_mockUnitOfWork.Object, _mockMongoUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task MigrateOnUpdateAsync_WhenEntityRequiresMigration_MigratesPublisher()
    {
        // Arrange
        const string publisherId = "publisherId";
        var publisherDto = new PublisherUpdateDto { Publisher = new PublisherUpdateInnerDto { CompanyName = "name", Id = publisherId } };

        _mockMongoUnitOfWork
            .Setup(u => u.Suppliers.ExistsAsync(It.IsAny<Expression<Func<Supplier, bool>>>()))
            .ReturnsAsync(true);

        _mockUnitOfWork
            .Setup(u => u.Publishers.ExistsAsync(It.IsAny<Expression<Func<Publisher, bool>>>()))
            .ReturnsAsync(false);

        var migratedPublisher = new Publisher();
        _mockMapper
            .Setup(m => m.Map<Publisher>(publisherDto))
            .Returns(migratedPublisher);

        _mockUnitOfWork
            .Setup(u => u.Publishers.AddAsync(migratedPublisher))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(u => u.SaveAsync())
            .Returns(Task.FromResult(1));

        // Act
        var result = await _service.MigrateOnUpdateAsync(publisherDto);

        // Assert
        Assert.Equal(publisherDto, result);
        _mockUnitOfWork.Verify(u => u.Publishers.AddAsync(migratedPublisher), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task MigrateOnUpdateAsync_WhenEntityDoesNotRequireMigration_ReturnsDto()
    {
        // Arrange
        var publisherId = Guid.Empty.ToString();
        var publisherDto = new PublisherUpdateDto { Publisher = new PublisherUpdateInnerDto { CompanyName = "name", Id = publisherId } };

        _mockUnitOfWork
            .Setup(u => u.SaveAsync())
            .Returns(Task.FromResult(1));

        // Act
        var result = await _service.MigrateOnUpdateAsync(publisherDto);

        // Assert
        Assert.Equal(publisherDto, result);
        _mockUnitOfWork.Verify(u => u.Publishers.ExistsAsync(It.IsAny<Expression<Func<Publisher, bool>>>()), Times.Never);
        _mockMongoUnitOfWork.Verify(u => u.Suppliers.ExistsAsync(It.IsAny<Expression<Func<Supplier, bool>>>()), Times.Never);
        _mockMapper.Verify(m => m.Map<Publisher>(publisherDto), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }
}