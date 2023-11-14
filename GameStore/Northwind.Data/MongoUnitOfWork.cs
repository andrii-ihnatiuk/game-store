using Northwind.Data.Entities;
using Northwind.Data.Interfaces;

namespace Northwind.Data;

public class MongoUnitOfWork : IMongoUnitOfWork
{
    private readonly IMongoContext _context;

    public MongoUnitOfWork(
        IMongoContext context,
        IGenericRepository<Order> ordersRepository,
        IGenericRepository<OrderDetail> orderDetailsRepository)
    {
        _context = context;
        Orders = ordersRepository;
        OrderDetails = orderDetailsRepository;
    }

    public IGenericRepository<Order> Orders { get; }

    public IGenericRepository<OrderDetail> OrderDetails { get; }

    public Task<bool> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}