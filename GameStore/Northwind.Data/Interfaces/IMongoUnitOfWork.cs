using Northwind.Data.Entities;

namespace Northwind.Data.Interfaces;

public interface IMongoUnitOfWork
{
    IOrderRepository Orders { get; }

    IOrderDetailRepository OrderDetails { get; }

    IProductRepository Products { get; }

    ICategoryRepository Categories { get; }

    ISupplierRepository Suppliers { get; }

    IGenericRepository<EntityLog> Logs { get; }

    Task<bool> SaveChangesAsync();
}