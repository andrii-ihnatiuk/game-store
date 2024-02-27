using System.Diagnostics.CodeAnalysis;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Northwind.Data.Entities;
using Northwind.Data.Interfaces;

namespace Northwind.Data.Repositories;

[ExcludeFromCodeCoverage]
public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
{
    public SupplierRepository(IMongoContext context)
        : base(context)
    {
    }

    public async Task<IList<Product>> GetProductsBySupplierIdAsync(string id)
    {
        var products = Context.GetCollection<Product>().AsQueryable();
        var queryResult = await DbSet.AsQueryable()
            .Where(s => s.Id == id)
            .GroupJoin(
                products,
                s => s.SupplierId,
                p => p.SupplierId,
                (s, p) => p.ToList())
            .SingleAsync();
        return queryResult;
    }
}