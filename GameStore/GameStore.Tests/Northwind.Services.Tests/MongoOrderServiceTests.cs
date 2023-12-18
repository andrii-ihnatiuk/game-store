using System.Linq.Expressions;
using AutoMapper;
using GameStore.Shared.DTOs.Order;
using Moq;
using Northwind.Data.Entities;
using Northwind.Data.Interfaces;
using Northwind.Services;

namespace GameStore.Tests.Northwind.Services.Tests;

public class MongoOrderServiceTests
{
    private readonly Mock<IMongoUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly MongoOrderService _service;

    public MongoOrderServiceTests()
    {
        _mockUnitOfWork = new Mock<IMongoUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _service = new MongoOrderService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetPaidOrdersByCustomerAsync_ReturnsOrderBriefDtoList()
    {
        // Arrange
        const string customerId = "customerId";
        var orders = new List<Order> { new() { CustomerId = customerId } };
        _mockUnitOfWork.Setup(u => u.Orders.GetAllAsync(It.IsAny<Expression<Func<Order, bool>>>()))
            .ReturnsAsync(orders);

        var expected = new List<OrderBriefDto> { new() { CustomerId = customerId } };
        _mockMapper.Setup(m => m.Map<IList<OrderBriefDto>>(orders)).Returns(expected);

        // Act
        var result = await _service.GetPaidOrdersByCustomerAsync(customerId, DateTime.MinValue, DateTime.MaxValue);

        // Assert
        Assert.Equal(expected.Count, result.Count);
    }

    [Fact]
    public async Task GetOrderByIdAsync_ReturnsOrderBriefDto()
    {
        // Arrange
        const string orderId = "orderId";
        var order = new Order { Id = orderId };
        _mockUnitOfWork.Setup(u => u.Orders.GetOneAsync(It.IsAny<Expression<Func<Order, bool>>>())).ReturnsAsync(order);

        var expected = new OrderBriefDto { Id = orderId };
        _mockMapper.Setup(m => m.Map<OrderBriefDto>(order)).Returns(expected);

        // Act
        var result = await _service.GetOrderByIdAsync(orderId);

        // Assert
        Assert.Equal(expected.Id, result.Id);
    }

    [Fact]
    public async Task GetOrderDetailsAsync_ReturnsOrderDetailDtoList()
    {
        // Arrange
        const string orderId = "orderId";
        var details = new List<OrderDetail> { new() { OrderId = 1234 } };
        _mockUnitOfWork.Setup(u => u.OrderDetails.GetAllByOrderObjectIdAsync(orderId)).ReturnsAsync(details);

        var expected = new List<OrderDetailDto> { new() };
        _mockMapper.Setup(m => m.Map<IList<OrderDetailDto>>(details)).Returns(expected);

        // Act
        var result = await _service.GetOrderDetailsAsync(orderId);

        // Assert
        Assert.Equal(expected.Count, result.Count);
    }
}