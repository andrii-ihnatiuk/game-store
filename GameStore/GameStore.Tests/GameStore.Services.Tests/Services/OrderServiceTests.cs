﻿using System.Linq.Expressions;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Services;
using GameStore.Shared.DTOs.Order;
using GameStore.Shared.Exceptions;
using GameStore.Shared.Models;
using GameStore.Shared.Options;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using Moq;

namespace GameStore.Tests.GameStore.Services.Tests.Services;

public class OrderServiceTests
{
    private const string GameAlias = "game";
    private static readonly string CustomerId = "customer-id";
    private static readonly Guid ProductId = Guid.NewGuid();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IOptions<TaxOptions>> _taxOptionsMock = new();
    private readonly CoreOrderService _service;

    public OrderServiceTests()
    {
        _taxOptionsMock.SetupGet(e => e.Value)
            .Returns(new TaxOptions { DefaultTax = 20 });
        _service = new CoreOrderService(_unitOfWork.Object, _mapper.Object, _taxOptionsMock.Object);
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
        var game = new Game { Price = 100, Id = ProductId, Alias = GameAlias, Name = "name" };
        var order = new Order()
        {
            Id = Guid.Empty,
            OrderDetails = new List<OrderDetail>()
            {
                new() { ProductId = ProductId, Quantity = 2, Product = game },
            },
        };
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
        var game = new Game { Price = 10, Id = ProductId, Alias = GameAlias, Name = "name" };
        var orderDetail = new OrderDetail { ProductName = GameAlias, Quantity = 2, Price = 20, Discount = 50, Product = game };
        var order = new Order { Sum = 20, OrderDetails = new List<OrderDetail> { orderDetail } };
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
        Assert.Equal(orderDetailDtos.Count, result.Details.Count);
    }

    [Fact]
    public async Task GetFilteredOrdersAsync_ReturnsCorrectAmountOfOrders()
    {
        // Arrange
        var orders = new List<Order> { new() };
        _unitOfWork.Setup(u => u.Orders.GetFilteredOrdersAsync(It.IsAny<OrdersFilter>()))
            .ReturnsAsync(orders);

        var orderDtos = new List<OrderBriefDto> { new() };
        _mapper.Setup(m => m.Map<IList<OrderBriefDto>>(orders))
            .Returns(orderDtos);

        // Act
        var result = await _service.GetFilteredOrdersAsync(new OrdersFilter());

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
        var result = await _service.GetOrderByIdAsync(Guid.Empty.ToString());

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
        var result = await _service.GetOrderDetailsAsync(Guid.Empty.ToString());

        // Assert
        Assert.Equal(orderDetailDtos.Count, result.Count);
    }

    [Fact]
    public async Task DeleteGameFromCartAsync_WhenQuantityIsOne_RemovesOrderDetail()
    {
        // Arrange
        var game = new Game { Price = 10, Id = ProductId, Alias = GameAlias, Name = "name" };
        var orderDetail = new OrderDetail { ProductName = GameAlias, Quantity = 1, Price = 20, Discount = 50, Product = game };
        var order = new Order { Sum = 20, OrderDetails = new List<OrderDetail> { orderDetail } };
        _unitOfWork.Setup(
                u => u.Orders.GetOneAsync(
                    It.IsAny<Expression<Func<Order, bool>>>(),
                    It.IsAny<Func<IQueryable<Order>, IIncludableQueryable<Order, object>>>(),
                    false))
            .ReturnsAsync(order);

        // Act
        await _service.DeleteGameFromCartAsync(CustomerId, GameAlias, false);

        // Assert
        Assert.DoesNotContain(orderDetail, order.OrderDetails);
        Assert.Equal(0, order.Sum);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteGameFromCartAsync_WhenQuantityGreaterThanOne_DecreasesQuantity()
    {
        // Arrange
        var game = new Game { Price = 10, Id = ProductId, Alias = GameAlias, Name = "name" };
        var orderDetail = new OrderDetail { ProductName = GameAlias, Quantity = 2, Price = 20, Discount = 50, Product = game };
        var order = new Order { Sum = 20, OrderDetails = new List<OrderDetail> { orderDetail } };

        _unitOfWork.Setup(
                u => u.Orders.GetOneAsync(
                    It.IsAny<Expression<Func<Order, bool>>>(),
                    It.IsAny<Func<IQueryable<Order>, IIncludableQueryable<Order, object>>>(),
                    false))
            .ReturnsAsync(order);

        // Act
        await _service.DeleteGameFromCartAsync(CustomerId, GameAlias, false);

        // Assert
        Assert.Equal(1, orderDetail.Quantity);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }
}