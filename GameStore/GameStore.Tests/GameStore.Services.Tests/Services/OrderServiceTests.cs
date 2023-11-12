using System.Linq.Expressions;
using AutoMapper;
using GameStore.Application.Services;
using GameStore.Data.Entities;
using GameStore.Data.Exceptions;
using GameStore.Data.Interfaces;
using GameStore.Shared.DTOs.Order;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace GameStore.Tests.GameStore.Services.Tests.Services;

public class OrderServiceTests
{
    private const string GameAlias = "game";
    private static readonly Guid CustomerId = Guid.NewGuid();
    private static readonly Guid ProductId = Guid.NewGuid();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly OrderService _service;

    public OrderServiceTests()
    {
        _service = new OrderService(_unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task AddGameToCartAsync_WhenOrderDoNotExist_CreatesNewOrder()
    {
        // Arrange
        var game = new Game { Price = 100, Id = ProductId, Alias = GameAlias, Name = "name" };
        _unitOfWork.Setup(u => u.Orders.GetOneAsync(
                It.IsAny<Expression<Func<Order, bool>>>(),
                It.IsAny<Func<IQueryable<Order>, IIncludableQueryable<Order, object>>>(),
                false))
            .Throws(new EntityNotFoundException("Not found"));

        _unitOfWork.Setup(u => u.Games.GetOneAsync(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Func<IQueryable<Game>, IIncludableQueryable<Game, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(game);

        // Act
        await _service.AddGameToCartAsync(CustomerId, GameAlias);

        // Assert
        _unitOfWork.Verify(u => u.Orders.AddAsync(It.IsAny<Order>()), Times.Once);
        _unitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task AddGameToCartAsync_WhenOrderExists_IncrementsQuantity()
    {
        // Arrange
        var order = new Order()
        {
            Id = Guid.Empty,
            OrderDetails = new List<OrderDetail>()
            {
                new() { ProductId = ProductId, Quantity = 2 },
            },
        };
        var game = new Game { Price = 100, Id = ProductId, Alias = GameAlias, Name = "name" };
        _unitOfWork.Setup(u => u.Orders.GetOneAsync(
                It.IsAny<Expression<Func<Order, bool>>>(),
                It.IsAny<Func<IQueryable<Order>, IIncludableQueryable<Order, object>>>(),
                false))
            .ReturnsAsync(order);

        _unitOfWork.Setup(u => u.Games.GetOneAsync(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Func<IQueryable<Game>, IIncludableQueryable<Game, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(game);

        // Act
        await _service.AddGameToCartAsync(CustomerId, GameAlias);

        // Assert
        Assert.Equal(3, order.OrderDetails.First().Quantity);
        _unitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task GetCartByCustomerAsync_ReturnsCorrectAmountOfOrderDetails()
    {
        // Arrange
        var order = new Order { OrderDetails = new List<OrderDetail> { new() } };
        _unitOfWork.Setup(u => u.Orders.GetOneAsync(
                It.IsAny<Expression<Func<Order, bool>>>(),
                It.IsAny<Func<IQueryable<Order>, IIncludableQueryable<Order, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(order);

        var orderDetailDtos = new List<OrderDetailDto> { new() };
        _mapper.Setup(m => m.Map<IList<OrderDetailDto>>(It.IsAny<IEnumerable<OrderDetail>>()))
            .Returns(orderDetailDtos);

        // Act
        var result = await _service.GetCartByCustomerAsync(CustomerId);

        // Assert
        Assert.Equal(orderDetailDtos.Count, result.Count);
    }

    [Fact]
    public async Task GetPaidOrdersByCustomerAsync_ReturnsCorrectAmountOfOrders()
    {
        // Arrange
        var orders = new List<Order> { new() };
        _unitOfWork.Setup(u => u.Orders.GetAsync(
                It.IsAny<Expression<Func<Order, bool>>>(),
                It.IsAny<Func<IQueryable<Order>, IOrderedQueryable<Order>>>(),
                It.IsAny<Func<IQueryable<Order>, IIncludableQueryable<Order, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(orders);

        var orderDtos = new List<OrderBriefDto> { new() };
        _mapper.Setup(m => m.Map<IList<OrderBriefDto>>(It.IsAny<IEnumerable<Order>>()))
            .Returns(orderDtos);

        // Act
        var result = await _service.GetPaidOrdersByCustomerAsync(CustomerId);

        // Assert
        Assert.Equal(orderDtos.Count, result.Count);
    }

    [Fact]
    public async Task GetOrderByIdAsync_ReturnsExpectedOrder()
    {
        // Arrange
        var order = new Order();
        var orderDto = new OrderBriefDto();
        _unitOfWork.Setup(u => u.Orders.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(order);
        _mapper.Setup(m => m.Map<OrderBriefDto>(order)).Returns(orderDto);

        // Act
        var result = await _service.GetOrderByIdAsync(Guid.Empty);

        // Assert
        Assert.Equal(orderDto, result);
    }

    [Fact]
    public async Task GetOrderDetailsAsync_ReturnsCorrectAmountOfOrderDetails()
    {
        // Arrange
        var order = new Order { OrderDetails = new List<OrderDetail> { new() } };
        _unitOfWork.Setup(u => u.Orders.GetOneAsync(
                It.IsAny<Expression<Func<Order, bool>>>(),
                It.IsAny<Func<IQueryable<Order>, IIncludableQueryable<Order, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(order);

        var orderDetailDtos = new List<OrderDetailDto> { new() };
        _mapper.Setup(m => m.Map<IList<OrderDetailDto>>(It.IsAny<IEnumerable<OrderDetail>>()))
            .Returns(orderDetailDtos);

        // Act
        var result = await _service.GetOrderDetailsAsync(Guid.Empty);

        // Assert
        Assert.Equal(orderDetailDtos.Count, result.Count);
    }

    [Fact]
    public async Task DeleteGameFromCartAsync_WhenQuantityIsOne_RemovesOrderDetail()
    {
        // Arrange
        var orderDetail = new OrderDetail { ProductName = GameAlias, Quantity = 1 };
        var order = new Order { Sum = 100, OrderDetails = new List<OrderDetail> { orderDetail } };

        _unitOfWork.Setup(
                u => u.Orders.GetOneAsync(
                    It.IsAny<Expression<Func<Order, bool>>>(),
                    It.IsAny<Func<IQueryable<Order>, IIncludableQueryable<Order, object>>>(),
                    false))
            .ReturnsAsync(order);

        // Act
        await _service.DeleteGameFromCartAsync(CustomerId, GameAlias);

        // Assert
        Assert.DoesNotContain(orderDetail, order.OrderDetails);
        Assert.Equal(0, order.Sum);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteGameFromCartAsync_WhenQuantityGreaterThanOne_DecreasesQuantity()
    {
        // Arrange
        var orderDetail = new OrderDetail { ProductName = GameAlias, Quantity = 2, Price = 20, Discount = 50 };
        var order = new Order { Sum = 20, OrderDetails = new List<OrderDetail> { orderDetail } };

        _unitOfWork.Setup(
                u => u.Orders.GetOneAsync(
                    It.IsAny<Expression<Func<Order, bool>>>(),
                    It.IsAny<Func<IQueryable<Order>, IIncludableQueryable<Order, object>>>(),
                    false))
            .ReturnsAsync(order);

        // Act
        await _service.DeleteGameFromCartAsync(CustomerId, GameAlias);

        // Assert
        Assert.Equal(1, orderDetail.Quantity);
        Assert.Equal(10, order.Sum);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }
}