using GameStore.Shared.Models;
using Northwind.Data.Entities;

namespace Northwind.Data.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<EntityFilteringResult<Product>> GetFilteredProductsAsync(GamesFilter filter);

    Task<IList<Category>> GetCategoriesByProductAliasAsync(string alias);

    Task<Supplier> GetSupplierByProductAliasAsync(string alias);
}