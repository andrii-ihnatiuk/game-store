using Northwind.Data.Entities;

namespace Northwind.Data.Interfaces;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<IList<Product>> GetProductsByCategoryId(string id);
}