using Northwind.Data.Entities;

namespace Northwind.Data.Interfaces;

public interface IMongoUnitOfWork
{
    IGenericRepository<Order> Orders { get; }

    IGenericRepository<OrderDetail> OrderDetails { get; }

    Task<bool> SaveChangesAsync();
}