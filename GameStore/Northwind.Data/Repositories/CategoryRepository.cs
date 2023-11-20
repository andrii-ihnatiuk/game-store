using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Northwind.Data.Entities;
using Northwind.Data.Interfaces;

namespace Northwind.Data.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(IMongoContext context)
        : base(context)
    {
    }

    public async Task<IList<Product>> GetProductsByCategoryId(string id)
    {
        var products = Context.GetCollection<Product>().AsQueryable();
        var queryResult = await DbSet.AsQueryable()
            .Where(c => c.Id == id)
            .GroupJoin(
                products,
                c => c.CategoryId,
                p => p.CategoryId,
                (c, p) => p.ToList())
            .SingleAsync();
        return queryResult;
    }
}