﻿using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Exceptions;
using GameStore.Data.Repositories;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Order;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddGameToCartAsync(Guid customerId, string gameAlias)
    {
        var order = await GetExistingOrderOrCreateNewAsync(customerId);
        await AddGameToOrderOrIncrementQuantity(order, gameAlias);
        await _unitOfWork.SaveAsync();
    }

    public async Task<IList<CartItemDto>> GetCartByCustomerAsync(Guid customerId)
    {
        var order = await GetExistingOrderOrCreateNewAsync(customerId, noTracking: true);
        return _mapper.Map<IList<CartItemDto>>(order.OrderDetails);
    }

    private async Task<Order> GetExistingOrderOrCreateNewAsync(Guid customerId, bool noTracking = false)
    {
        Order order;
        try
        {
            order = await _unitOfWork.Orders.GetOneAsync(
                predicate: o => o.CustomerId == customerId && o.PaidDate == null,
                include: q => q.Include(o => o.OrderDetails),
                noTracking: noTracking);
        }
        catch (EntityNotFoundException)
        {
            order = new Order()
            {
                CustomerId = customerId,
                Sum = 0,
            };
            if (!noTracking)
            {
                await _unitOfWork.Orders.AddAsync(order);
            }
        }

        return order;
    }

    private async Task AddGameToOrderOrIncrementQuantity(Order order, string gameAlias)
    {
        var game = await _unitOfWork.Games.GetOneAsync(g => g.Alias == gameAlias);
        var orderDetail = order.OrderDetails.FirstOrDefault(od => od.ProductId == game.Id);

        if (orderDetail is not null)
        {
            orderDetail.Quantity += 1;
        }
        else
        {
            order.OrderDetails.Add(new OrderDetail()
            {
                OrderId = order.Id,
                ProductId = game.Id,
                Price = game.Price,
                Quantity = 1,
                ProductName = game.Name,
                Discount = 0,
            });
        }

        order.Sum = order.OrderDetails.Select(d => d.Price * d.Quantity).Sum();
    }
}