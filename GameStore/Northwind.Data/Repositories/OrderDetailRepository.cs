using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Northwind.Data.Entities;
using Northwind.Data.Interfaces;

namespace Northwind.Data.Repositories;

public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
{
    public OrderDetailRepository(IMongoContext context)
        : base(context)
    {
    }

    public async Task<IList<OrderDetail>> GetAllByOrderObjectId(string id)
    {
        var orders = Context.GetCollection<Order>().AsQueryable();
        var details = DbSet.AsQueryable();

        var query = await orders.Where(o => o.Id == id)
            .GroupJoin(
                details,
                o => o.OrderId,
                d => d.OrderId,
                (o, d) => d.ToList())
            .SingleAsync();
        return query;
    }
}