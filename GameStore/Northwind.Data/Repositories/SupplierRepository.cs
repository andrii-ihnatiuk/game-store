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

    public async Task<IList<Product>> GetProductsBySupplierNameAsync(string companyName)
    {
        var products = Context.GetCollection<Product>().AsQueryable();
        var queryResult = await DbSet.AsQueryable()
            .Where(c => c.CompanyName == companyName)
            .GroupJoin(
                products,
                c => c.SupplierId,
                p => p.SupplierId,
                (c, p) => p.ToList())
            .SingleAsync();
        return queryResult;
    }
}