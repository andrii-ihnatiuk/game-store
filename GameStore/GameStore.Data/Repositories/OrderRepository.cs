using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(GameStoreDbContext context)
        : base(context)
    {
    }

    public async Task<IList<Order>> GetFilteredOrdersAsync(OrdersFilter filter)
    {
        var query = DbSet.AsQueryable().Where(o => o.Status != OrderStatus.Checkout);
        if (filter.CustomerId is not null)
        {
            query = query.Where(o => o.CustomerId == filter.CustomerId);
        }

        if (filter.OnlyPaid)
        {
            query = query.Where(o => o.Status == OrderStatus.Paid);
        }

        return await query.Where(o => o.OrderDate >= filter.LowerDate && o.OrderDate <= filter.UpperDate).ToListAsync();
    }
}