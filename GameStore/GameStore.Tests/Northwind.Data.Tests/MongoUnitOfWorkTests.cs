using Moq;
using Northwind.Data;
using Northwind.Data.Entities;
using Northwind.Data.Interfaces;

namespace GameStore.Tests.Northwind.Data.Tests;

public class MongoUnitOfWorkTests
{
    private readonly Mock<IMongoContext> _contextMock = new();
    private readonly Mock<IGenericRepository<Order>> _orderRepoMock = new();
    private readonly Mock<IOrderDetailRepository> _orderDetailsRepoMock = new();
    private readonly Mock<IGenericRepository<Product>> _productRepoMock = new();
    private readonly Mock<ICategoryRepository> _categoryRepoMock = new();
    private readonly Mock<ISupplierRepository> _supplierRepoMock = new();
    private readonly Mock<IGenericRepository<EntityLog>> _entityLogsRepoMock = new();
    private readonly MongoUnitOfWork _mongoUnitOfWork;

    public MongoUnitOfWorkTests()
    {
        _mongoUnitOfWork = new MongoUnitOfWork(
            _contextMock.Object,
            _orderRepoMock.Object,
            _orderDetailsRepoMock.Object,
            _productRepoMock.Object,
            _categoryRepoMock.Object,
            _supplierRepoMock.Object,
            _entityLogsRepoMock.Object);
    }

    [Fact]
    public async Task SaveChangesAsync_WhenCalled_CallsContext()
    {
        // Arrange
        _contextMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(true).Verifiable();

        // Act
        await _mongoUnitOfWork.SaveChangesAsync();

        // Assert
        _contextMock.Verify(c => c.SaveChangesAsync(), Times.Once);
    }
}