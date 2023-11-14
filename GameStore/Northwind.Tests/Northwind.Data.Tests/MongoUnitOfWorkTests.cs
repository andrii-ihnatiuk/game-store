using Moq;
using Northwind.Data;
using Northwind.Data.Entities;
using Northwind.Data.Interfaces;

namespace Northwind.Tests.Northwind.Data.Tests;

public class MongoUnitOfWorkTests
{
    private readonly Mock<IMongoContext> _contextMock = new();
    private readonly Mock<IGenericRepository<Order>> _orderRepoMock = new();
    private readonly Mock<IGenericRepository<OrderDetail>> _orderDetailsRepoMock = new();
    private readonly MongoUnitOfWork _mongoUnitOfWork;

    public MongoUnitOfWorkTests()
    {
        _mongoUnitOfWork = new MongoUnitOfWork(
            _contextMock.Object,
            _orderRepoMock.Object,
            _orderDetailsRepoMock.Object);
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