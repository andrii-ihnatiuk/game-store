using GameStore.Shared.Models;
using Northwind.Data.Entities;

namespace Northwind.Data.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IList<Order>> GetFilteredOrdersAsync(OrdersFilter filter);
}