using GameStore.Shared.DTOs.Order;

namespace Northwind.Services.Interfaces;

public interface IOrderService
{
    Task<IList<OrderBriefDto>> GetPaidOrdersByCustomerAsync(string customerId);
}