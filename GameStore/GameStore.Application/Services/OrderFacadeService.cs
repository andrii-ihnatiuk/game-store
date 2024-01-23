using GameStore.Application.Interfaces;
using GameStore.Application.Interfaces.Util;
using GameStore.Services.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Order;
using GameStore.Shared.Exceptions;
using GameStore.Shared.Extensions;
using GameStore.Shared.Interfaces.Services;
using GameStore.Shared.Models;

namespace GameStore.Application.Services;

public class OrderFacadeService : IOrderFacadeService
{
    private readonly IEntityServiceResolver _serviceResolver;

    public OrderFacadeService(IEntityServiceResolver serviceResolver)
    {
        _serviceResolver = serviceResolver;
    }

    public async Task<IList<OrderBriefDto>> GetFilteredOrdersAsync(OrdersFilter filter)
    {
        var services = _serviceResolver.ResolveAll<IOrderService>();
        var tasks = services.Select(s => s.GetFilteredOrdersAsync(filter)).ToList();
        await Task.WhenAll(tasks);

        var orders = tasks.SelectMany(t => t.Result).OrderByDescending(o => o.OrderDate).ToList();
        return orders;
    }

    public async Task<OrderBriefDto> GetOrderByIdAsync(string orderId)
    {
        var orderService = _serviceResolver.ResolveForEntityId<IOrderService>(orderId);
        var dto = await orderService.GetOrderByIdAsync(orderId);
        return dto;
    }

    public async Task<IList<OrderDetailDto>> GetOrderDetailsAsync(string orderId)
    {
        var orderService = _serviceResolver.ResolveForEntityId<IOrderService>(orderId);
        var dto = await orderService.GetOrderDetailsAsync(orderId);
        return dto;
    }

    public async Task ShipOrderAsync(string orderId)
    {
        if (orderId.IsNotGuidFormat())
        {
            throw new GameStoreNotSupportedException();
        }

        var orderService = _serviceResolver.ResolveAll<ICoreOrderService>().Single();
        await orderService.UpdateOrderStatusAsync(orderId, OrderStatus.Shipped);
    }
}