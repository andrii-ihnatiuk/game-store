using GameStore.Shared.DTOs.Order;
using GameStore.Shared.Models;

namespace GameStore.Shared.Interfaces.Services;

public interface IOrderService : IResolvableByEntityStorage
{
    Task<IList<OrderBriefDto>> GetFilteredOrdersAsync(OrdersFilter filter);

    Task<OrderBriefDto> GetOrderByIdAsync(string orderId);

    Task<IList<OrderDetailDto>> GetOrderDetailsAsync(string orderId);
}