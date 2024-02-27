using Northwind.Data.Entities;

namespace Northwind.Data.Interfaces;

public interface ISupplierRepository : IGenericRepository<Supplier>
{
    Task<IList<Product>> GetProductsBySupplierIdAsync(string id);
}