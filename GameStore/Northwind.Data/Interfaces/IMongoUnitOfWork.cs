using Northwind.Data.Entities;

namespace Northwind.Data.Interfaces;

public interface IMongoUnitOfWork
{
    IGenericRepository<Order> Orders { get; }

    IOrderDetailRepository OrderDetails { get; }

    IGenericRepository<Product> Products { get; }

    ICategoryRepository Categories { get; }

    Task<bool> SaveChangesAsync();
}