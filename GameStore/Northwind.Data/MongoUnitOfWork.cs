using Northwind.Data.Entities;
using Northwind.Data.Interfaces;

namespace Northwind.Data;

public class MongoUnitOfWork : IMongoUnitOfWork
{
    private readonly IMongoContext _context;

    public MongoUnitOfWork(
        IMongoContext context,
        IGenericRepository<Order> ordersRepository)
    {
        _context = context;
        Orders = ordersRepository;
    }

    public IGenericRepository<Order> Orders { get; }

    public Task<bool> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}