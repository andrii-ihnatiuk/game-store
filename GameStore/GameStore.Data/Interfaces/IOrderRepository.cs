using GameStore.Data.Entities;
using GameStore.Shared.Models;

namespace GameStore.Data.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IList<Order>> GetFilteredOrdersAsync(OrdersFilter filter);
}