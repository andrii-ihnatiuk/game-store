using GameStore.Shared.DTOs.Order;

namespace GameStore.Application.Interfaces;

public interface IOrderFacadeService
{
    Task<IList<OrderBriefDto>> GetOrdersHistoryByCustomerAsync(string customerId, DateTime lowerDate, DateTime upperDate);

    Task<OrderBriefDto> GetOrderByIdAsync(string orderId);

    Task<IList<OrderDetailDto>> GetOrderDetailsAsync(string orderId);
}