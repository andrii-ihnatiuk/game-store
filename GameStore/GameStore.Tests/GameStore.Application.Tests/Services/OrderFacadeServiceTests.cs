using GameStore.Application.Interfaces;
using GameStore.Application.Services;
using GameStore.Shared.DTOs.Order;
using GameStore.Shared.Interfaces.Services;
using Moq;

namespace GameStore.Tests.GameStore.Application.Tests.Services;

public class OrderFacadeServiceTests
{
    private readonly Mock<IOrderService> _coreOrderServiceMock;
    private readonly Mock<IOrderService> _mongoOrderServiceMock;
    private readonly Mock<IServiceResolver> _serviceResolver;
    private readonly OrderFacadeService _service;

    public OrderFacadeServiceTests()
    {
        _coreOrderServiceMock = new Mock<IOrderService>();
        _mongoOrderServiceMock = new Mock<IOrderService>();
        _serviceResolver = new Mock<IServiceResolver>();

        _service = new OrderFacadeService(_serviceResolver.Object);
    }

    [Fact]
    public async Task GetOrdersHistoryByCustomerAsync_GivenCustomerIdAndDateRange_ShouldReturnOrdersInDescendingOrder()
    {
        // Arrange
        const string customerId = "someCustomerId";
        var orders = new List<OrderBriefDto>
        {
            new() { OrderDate = DateTime.Now },
            new() { OrderDate = DateTime.Now.AddDays(-2) },
            new() { OrderDate = DateTime.MaxValue },
            new() { OrderDate = DateTime.MinValue },
        };
        _coreOrderServiceMock
            .Setup(s => s.GetPaidOrdersByCustomerAsync(customerId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(orders.Take(3).ToList());
        _mongoOrderServiceMock
            .Setup(s => s.GetPaidOrdersByCustomerAsync(customerId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(orders.TakeLast(1).ToList());

        _serviceResolver.Setup(s => s.ResolveAll<IOrderService>())
            .Returns(new List<IOrderService> { _coreOrderServiceMock.Object, _mongoOrderServiceMock.Object });

        // Act
        var result = await _service.GetOrdersHistoryByCustomerAsync(customerId, DateTime.MinValue, DateTime.MaxValue);

        // Assert
        Assert.Equal(orders.Count, result.Count);
        Assert.Equal(orders[2].OrderDate, result.First().OrderDate);
        Assert.Equal(orders[3].OrderDate, result.Last().OrderDate);
    }

    [Fact]
    public async Task GetOrderByIdAsync_GivenOrderId_ShouldReturnOrder()
    {
        // Arrange
        const string orderId = "someOrderId";
        var order = new OrderBriefDto { Id = orderId };
        _serviceResolver.Setup(s => s.ResolveForEntityId<IOrderService>(orderId))
            .Returns(_coreOrderServiceMock.Object)
            .Verifiable();
        _coreOrderServiceMock.Setup(s => s.GetOrderByIdAsync(orderId))
            .ReturnsAsync(order)
            .Verifiable();

        // Act
        var result = await _service.GetOrderByIdAsync(orderId);

        // Assert
        _serviceResolver.Verify(p => p.ResolveForEntityId<IOrderService>(orderId), Times.Once);
        _coreOrderServiceMock.Verify(s => s.GetOrderByIdAsync(orderId), Times.Once);
        Assert.Equal(order.Id, result.Id);
    }

    [Fact]
    public async Task GetOrderDetailsAsync_GivenOrderId_ShouldReturnOrderDetails()
    {
        // Arrange
        const string orderId = "someOrderId";
        var orderDetails = new List<OrderDetailDto> { new(), new() };
        _serviceResolver.Setup(s => s.ResolveForEntityId<IOrderService>(orderId))
            .Returns(_coreOrderServiceMock.Object)
            .Verifiable();
        _coreOrderServiceMock.Setup(s => s.GetOrderDetailsAsync(orderId))
            .ReturnsAsync(orderDetails)
            .Verifiable();

        // Act
        var result = await _service.GetOrderDetailsAsync(orderId);

        // Assert
        _coreOrderServiceMock.Verify(s => s.GetOrderDetailsAsync(orderId), Times.Once);
        _serviceResolver.Verify(s => s.ResolveForEntityId<IOrderService>(orderId), Times.Once);
        Assert.Equal(orderDetails.Count, result.Count);
    }
}