using System.Diagnostics.CodeAnalysis;
using GameStore.Shared.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Northwind.Data.Entities;
using Northwind.Data.Interfaces;

namespace Northwind.Data.Repositories;

[ExcludeFromCodeCoverage]
public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(IMongoContext context)
        : base(context)
    {
    }

    public async Task<IList<Order>> GetFilteredOrdersAsync(OrdersFilter filter)
    {
        var query = DbSet.AsQueryable();
        if (filter.CustomerId is not null)
        {
            query = query.Where(o => o.CustomerId == filter.CustomerId);
        }

        return await query.Where(o => o.OrderDate >= filter.LowerDate && o.OrderDate <= filter.UpperDate).ToListAsync();
    }
}