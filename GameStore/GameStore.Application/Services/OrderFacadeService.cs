using GameStore.Application.Interfaces;
using GameStore.Shared.DTOs.Order;

namespace GameStore.Application.Services;

public class OrderFacadeService : IOrderFacadeService
{
    private readonly IOrderServiceProvider _orderServiceProvider;

    public OrderFacadeService(IOrderServiceProvider orderServiceProvider)
    {
        _orderServiceProvider = orderServiceProvider;
    }

    public async Task<IList<OrderBriefDto>> GetOrdersHistoryByCustomerAsync(string customerId, DateTime lowerDate, DateTime upperDate)
    {
        var services = _orderServiceProvider.GetAll();
        var tasks = services.Select(s => s.GetPaidOrdersByCustomerAsync(customerId, lowerDate, upperDate)).ToList();
        await Task.WhenAll(tasks);

        var orders = tasks.SelectMany(t => t.Result).OrderByDescending(o => o.OrderDate).ToList();
        return orders;
    }

    public async Task<OrderBriefDto> GetOrderByIdAsync(string orderId)
    {
        var orderService = _orderServiceProvider.GetByIdString(orderId);
        var dto = await orderService.GetOrderByIdAsync(orderId);
        return dto;
    }

    public async Task<IList<OrderDetailDto>> GetOrderDetailsAsync(string orderId)
    {
        var orderService = _orderServiceProvider.GetByIdString(orderId);
        var dto = await orderService.GetOrderDetailsAsync(orderId);
        return dto;
    }
}