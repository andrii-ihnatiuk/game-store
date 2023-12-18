using GameStore.Application.Interfaces;
using GameStore.Shared.DTOs.Order;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Application.Services;

public class OrderFacadeService : IOrderFacadeService
{
    private readonly IServiceResolver _serviceResolver;

    public OrderFacadeService(IServiceResolver serviceResolver)
    {
        _serviceResolver = serviceResolver;
    }

    public async Task<IList<OrderBriefDto>> GetOrdersHistoryByCustomerAsync(string customerId, DateTime lowerDate, DateTime upperDate)
    {
        var services = _serviceResolver.ResolveAll<IOrderService>();
        var tasks = services.Select(s => s.GetPaidOrdersByCustomerAsync(customerId, lowerDate, upperDate)).ToList();
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
}