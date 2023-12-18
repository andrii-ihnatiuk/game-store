using GameStore.Shared.DTOs.Order;

namespace GameStore.Shared.Interfaces.Services;

public interface IOrderService : IResolvableByEntityStorage
{
    Task<IList<OrderBriefDto>> GetPaidOrdersByCustomerAsync(string customerId, DateTime lowerDate, DateTime upperDate);

    Task<OrderBriefDto> GetOrderByIdAsync(string orderId);

    Task<IList<OrderDetailDto>> GetOrderDetailsAsync(string orderId);
}