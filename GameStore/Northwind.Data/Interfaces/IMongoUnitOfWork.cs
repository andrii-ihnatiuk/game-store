using Northwind.Data.Entities;

namespace Northwind.Data.Interfaces;

public interface IMongoUnitOfWork
{
    IGenericRepository<Order> Orders { get; }

    Task<bool> SaveChangesAsync();
}