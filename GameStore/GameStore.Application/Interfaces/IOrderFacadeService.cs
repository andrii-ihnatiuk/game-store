using GameStore.Shared.DTOs.Order;
using GameStore.Shared.Models;

namespace GameStore.Application.Interfaces;

public interface IOrderFacadeService
{
    Task<IList<OrderBriefDto>> GetFilteredOrdersAsync(OrdersFilter filter);

    Task<OrderBriefDto> GetOrderByIdAsync(string orderId);

    Task<IList<OrderDetailDto>> GetOrderDetailsAsync(string orderId);

    Task ShipOrderAsync(string orderId);
}