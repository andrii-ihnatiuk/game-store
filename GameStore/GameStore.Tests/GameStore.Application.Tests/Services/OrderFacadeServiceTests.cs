using GameStore.Application.Interfaces.Util;
using GameStore.Application.Services;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Order;
using GameStore.Shared.Interfaces.Services;
using GameStore.Shared.Models;
using Moq;

namespace GameStore.Tests.GameStore.Application.Tests.Services;

public class OrderFacadeServiceTests
{
    private readonly Mock<ICoreOrderService> _coreOrderServiceMock;
    private readonly Mock<IOrderService> _mongoOrderServiceMock;
    private readonly Mock<IEntityServiceResolver> _serviceResolver;
    private readonly OrderFacadeService _service;

    public OrderFacadeServiceTests()
    {
        _coreOrderServiceMock = new Mock<ICoreOrderService>();
        _mongoOrderServiceMock = new Mock<IOrderService>();
        _serviceResolver = new Mock<IEntityServiceResolver>();

        _serviceResolver.Setup(s => s.ResolveForEntityId<IOrderService>(It.IsAny<string>()))
            .Returns(_coreOrderServiceMock.Object);
        _serviceResolver.Setup(s => s.ResolveAll<IOrderService>())
            .Returns(new List<IOrderService> { _coreOrderServiceMock.Object, _mongoOrderServiceMock.Object });
        _serviceResolver.Setup(s => s.ResolveAll<ICoreOrderService>())
            .Returns(new List<ICoreOrderService> { _coreOrderServiceMock.Object });

        _service = new OrderFacadeService(_serviceResolver.Object);
    }

    [Fact]
    public async Task GetFilteredOrdersAsync_GivenFilter_ShouldReturnOrdersInDescendingOrder()
    {
        // Arrange
        var orders = new List<OrderBriefDto>
        {
            new() { OrderDate = DateTime.Now },
            new() { OrderDate = DateTime.Now.AddDays(-2) },
            new() { OrderDate = DateTime.MaxValue },
            new() { OrderDate = DateTime.MinValue },
        };
        _coreOrderServiceMock
            .Setup(s => s.GetFilteredOrdersAsync(It.IsAny<OrdersFilter>()))
            .ReturnsAsync(orders.Take(3).ToList());
        _mongoOrderServiceMock
            .Setup(s => s.GetFilteredOrdersAsync(It.IsAny<OrdersFilter>()))
            .ReturnsAsync(orders.TakeLast(1).ToList());

        // Act
        var result = await _service.GetFilteredOrdersAsync(new OrdersFilter());

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
        _coreOrderServiceMock.Setup(s => s.GetOrderByIdAsync(orderId))
            .ReturnsAsync(order)
            .Verifiable(Times.Once);

        // Act
        var result = await _service.GetOrderByIdAsync(orderId);

        // Assert
        _coreOrderServiceMock.Verify();
        Assert.Equal(order.Id, result.Id);
    }

    [Fact]
    public async Task GetOrderDetailsAsync_GivenOrderId_ShouldReturnOrderDetails()
    {
        // Arrange
        const string orderId = "someOrderId";
        var orderDetails = new List<OrderDetailDto> { new(), new() };
        _coreOrderServiceMock.Setup(s => s.GetOrderDetailsAsync(orderId))
            .ReturnsAsync(orderDetails)
            .Verifiable(Times.Once);

        // Act
        var result = await _service.GetOrderDetailsAsync(orderId);

        // Assert
        _coreOrderServiceMock.Verify();
        Assert.Equal(orderDetails.Count, result.Count);
    }

    [Fact]
    public async Task ShipOrderAsync_GivenGuidOrderId_CallsOrderService()
    {
        // Arrange
        var orderId = Guid.Empty.ToString();
        _coreOrderServiceMock.Setup(s => s.ShipOrderAsync(orderId))
            .Returns(Task.CompletedTask).Verifiable(Times.Once);

        // Act
        await _service.ShipOrderAsync(orderId);

        // Assert
        _coreOrderServiceMock.Verify();
    }
}