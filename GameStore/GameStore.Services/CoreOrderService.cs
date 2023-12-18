using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Order;
using GameStore.Shared.Exceptions;
using GameStore.Shared.Util;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services;

public class CoreOrderService : CoreServiceBase, ICoreOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CoreOrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddGameToCartAsync(string customerId, string gameAlias)
    {
        ThrowIfGameIsFromNorthwind(gameAlias);
        var order = await GetExistingOrderOrCreateNewAsync(customerId);
        await AddGameToOrderOrIncrementQuantityAsync(order, gameAlias);
        RecalculateTotalSumFor(order);
        await _unitOfWork.SaveAsync();
    }

    public async Task<IList<OrderDetailDto>> GetCartByCustomerAsync(string customerId)
    {
        var order = await GetExistingOrderOrCreateNewAsync(customerId, noTracking: true);
        return _mapper.Map<IList<OrderDetailDto>>(order.OrderDetails);
    }

    public async Task<IList<OrderBriefDto>> GetPaidOrdersByCustomerAsync(string customerId, DateTime lowerDate, DateTime upperDate)
    {
        var paidOrders = await _unitOfWork.Orders.GetAsync(
            o => o.CustomerId == customerId
                 && o.PaidDate != null
                 && o.OrderDate >= lowerDate
                 && o.OrderDate <= upperDate);
        return _mapper.Map<IList<OrderBriefDto>>(paidOrders);
    }

    public async Task<OrderBriefDto> GetOrderByIdAsync(string orderId)
    {
        var id = Guid.Parse(orderId);
        var order = await _unitOfWork.Orders.GetByIdAsync(id);
        return _mapper.Map<OrderBriefDto>(order);
    }

    public async Task<IList<OrderDetailDto>> GetOrderDetailsAsync(string orderId)
    {
        var id = Guid.Parse(orderId);
        var order = await _unitOfWork.Orders.GetOneAsync(
            predicate: o => o.Id == id,
            include: q => q.Include(o => o.OrderDetails));
        return _mapper.Map<IList<OrderDetailDto>>(order.OrderDetails);
    }

    public async Task DeleteGameFromCartAsync(string customerId, string gameAlias)
    {
        var order = await _unitOfWork.Orders.GetOneAsync(
            predicate: o => o.CustomerId == customerId && o.PaidDate == null,
            include: q => q.Include(o => o.OrderDetails),
            noTracking: false);

        DeleteGameFromOrderOrDecrementQuantity(order, gameAlias);
        RecalculateTotalSumFor(order);
        await _unitOfWork.SaveAsync();
    }

    private static void ThrowIfGameIsFromNorthwind(string alias)
    {
        if (EntityAliasUtil.ContainsSuffix(alias))
        {
            throw new OrderFromNorthwindException();
        }
    }

    private async Task<Order> GetExistingOrderOrCreateNewAsync(string customerId, bool noTracking = false)
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
                OrderDate = DateTime.UtcNow,
            };
            if (!noTracking)
            {
                await _unitOfWork.Orders.AddAsync(order);
            }
        }

        return order;
    }

    private async Task AddGameToOrderOrIncrementQuantityAsync(Order order, string gameAlias)
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
                ProductName = game.Alias,
                Discount = 0,
            });
        }
    }

    private static void RecalculateTotalSumFor(Order order)
    {
        order.Sum = order.OrderDetails
            .Select(d => (d.Price * d.Quantity) - (d.Price * d.Quantity * (decimal)d.Discount / 100)).Sum();
    }

    private static void DeleteGameFromOrderOrDecrementQuantity(Order order, string gameAlias)
    {
        var orderDetail = order.OrderDetails.First(d => d.ProductName == gameAlias);
        if (orderDetail.Quantity > 1)
        {
            orderDetail.Quantity -= 1;
        }
        else
        {
            order.OrderDetails.Remove(orderDetail);
        }
    }
}