using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Services.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Order;
using GameStore.Shared.Exceptions;
using GameStore.Shared.Models;
using GameStore.Shared.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GameStore.Services;

public class CoreOrderService : CoreServiceBase, ICoreOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly TaxOptions _taxOptions;

    public CoreOrderService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<TaxOptions> taxOptions)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _taxOptions = taxOptions.Value;
    }

    public async Task AddGameToCartAsync(string customerId, string gameAlias)
    {
        var order = await GetExistingOrderOrCreateNewAsync(customerId);
        await AddGameToOrderOrIncrementQuantityAsync(order, gameAlias);
        RecalculateOrderPricing(order, _taxOptions);
        await _unitOfWork.SaveAsync();
    }

    public async Task<CartDetailsDto> GetCartByCustomerAsync(string customerId)
    {
        var order = await GetExistingOrderOrCreateNewAsync(customerId, noTracking: true);
        var cartDetails = CreateCartDetailsFromOrder(order);
        cartDetails.Details = _mapper.Map<IList<OrderDetailDto>>(order.OrderDetails);
        return cartDetails;
    }

    public async Task<Order> GetOrderForProcessingAsync(string customerId, bool noTracking = false)
    {
        var order = await _unitOfWork.Orders.GetOneAsync(
            predicate: o => o.CustomerId == customerId && o.PaidDate == null,
            include: q => q.Include(o => o.OrderDetails).ThenInclude(od => od.Product),
            noTracking: noTracking);

        RecalculateOrderPricing(order, _taxOptions);
        RefreshProductsRelatedInfo(order.OrderDetails);

        return order;
    }

    public async Task ShipOrderAsync(string orderId)
    {
        var id = Guid.Parse(orderId);
        var order = await _unitOfWork.Orders.GetByIdAsync(id);
        order.ShippedDate = DateTime.UtcNow;
        order.Status = OrderStatus.Shipped;
        await _unitOfWork.SaveAsync();
    }

    public async Task<IList<OrderBriefDto>> GetFilteredOrdersAsync(OrdersFilter filter)
    {
        var paidOrders = await _unitOfWork.Orders.GetFilteredOrdersAsync(filter);
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

    public async Task DeleteGameFromCartAsync(string customerId, string gameAlias, bool deleteAll)
    {
        var order = await GetOrderForProcessingAsync(customerId);
        DeleteGameFromOrderOrDecrementQuantity(order, gameAlias, deleteAll);
        RecalculateOrderPricing(order, _taxOptions);
        await _unitOfWork.SaveAsync();
    }

    private static void RecalculateOrderPricing(Order order, TaxOptions taxOptions)
    {
        order.Sum = 0;
        foreach (var d in order.OrderDetails)
        {
            d.Price = d.Product.Price * d.Quantity;
            d.Discount = d.Product.Discount;
            d.FinalPrice = CalculateProductFinalPrice(d.Product.Price, d.Quantity, d.Product.Discount);
            order.Sum += d.FinalPrice + (d.FinalPrice * taxOptions.DefaultTax / 100);
        }
    }

    private static void RefreshProductsRelatedInfo(IEnumerable<OrderDetail> details)
    {
        foreach (var d in details)
        {
            d.ProductName = d.Product.Alias;
        }
    }

    private static decimal CalculateProductFinalPrice(decimal price, uint quantity, uint discountPercentage)
    {
        return price * quantity * (100 - discountPercentage) / 100;
    }

    private static CartDetailsDto CreateCartDetailsFromOrder(Order order)
    {
        var cartDetails = new CartDetailsDto
        {
            Subtotal = order.OrderDetails.Select(d => d.FinalPrice).Sum(),
            Total = order.Sum,
        };
        cartDetails.Taxes = order.Sum - cartDetails.Subtotal;

        return cartDetails;
    }

    private async Task<Order> GetExistingOrderOrCreateNewAsync(string customerId, bool noTracking = false)
    {
        Order order;
        try
        {
            order = await GetOrderForProcessingAsync(customerId, noTracking);
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
        var game = await _unitOfWork.Games.GetOneAsync(g => g.Alias == gameAlias, noTracking: false);
        if (game.Deleted)
        {
            throw new GameStoreNotSupportedException("Cannot buy a deleted game!");
        }

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
                ProductName = game.Alias,
                Product = game,
                Price = game.Price,
                FinalPrice = CalculateProductFinalPrice(game.Price, quantity: 1, game.Discount),
                Discount = game.Discount,
                Quantity = 1,
            });
        }
    }

    private static void DeleteGameFromOrderOrDecrementQuantity(Order order, string gameAlias, bool deleteAll)
    {
        var orderDetail = order.OrderDetails.First(d => d.ProductName == gameAlias);
        if (orderDetail.Quantity > 1 && !deleteAll)
        {
            orderDetail.Quantity -= 1;
        }
        else
        {
            order.OrderDetails.Remove(orderDetail);
        }
    }
}