using Moq;
using Northwind.Data.Entities;
using Northwind.Data.Interfaces;
using Northwind.Data.Logger;

namespace GameStore.Tests.Northwind.Services.Tests;

public class EntityLoggerTests
{
    private readonly Mock<IMongoUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IGenericRepository<EntityLog>> _entityLogRepoMock;
    private readonly EntityLogger _entityLogger;

    public EntityLoggerTests()
    {
        _mockUnitOfWork = new Mock<IMongoUnitOfWork>();
        _entityLogRepoMock = new Mock<IGenericRepository<EntityLog>>();
        _entityLogRepoMock.Setup(e => e.Add(It.IsAny<EntityLog>())).Verifiable();
        _mockUnitOfWork.Setup(u => u.Logs).Returns(_entityLogRepoMock.Object);
        _entityLogger = new EntityLogger(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task LogCreateAsync_CallsAddAndSaveChangesAsync()
    {
        // Arrange
        var entity = new { Property = "value" };

        // Act
        await _entityLogger.LogCreateAsync(entity);

        // Assert
        _entityLogRepoMock.Verify(r => r.Add(It.IsAny<EntityLog>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task LogUpdateAsync_CallsAddAndSaveChangesAsync()
    {
        // Arrange
        var oldEntity = new { Property = "oldValue" };
        var updEntity = new { Property = "newValue" };

        // Act
        await _entityLogger.LogUpdateAsync(oldEntity, updEntity);

        // Assert
        _entityLogRepoMock.Verify(r => r.Add(It.IsAny<EntityLog>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task LogDeleteAsync_CallsAddAndSaveChangesAsync()
    {
        // Arrange
        var entity = new { Property = "value" };

        // Act
        await _entityLogger.LogDeleteAsync(entity);

        // Assert
        _entityLogRepoMock.Verify(r => r.Add(It.IsAny<EntityLog>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}