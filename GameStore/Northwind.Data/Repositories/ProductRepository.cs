using System.Diagnostics.CodeAnalysis;
using GameStore.Shared.Constants.Filter;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Northwind.Data.Entities;
using Northwind.Data.Interfaces;

namespace Northwind.Data.Repositories;

[ExcludeFromCodeCoverage]
public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(IMongoContext context)
        : base(context)
    {
    }

    public async Task<EntityFilteringResult<Product>> GetFilteredProductsAsync(GamesFilter filter)
    {
        var query = DbSet.AsQueryable().Where(p => !filter.Blacklist.Contains(p.Id));

        query = filter.MaxPrice is null ? query : query.Where(p => p.UnitPrice <= filter.MaxPrice);
        query = filter.MinPrice is null ? query : query.Where(p => p.UnitPrice >= filter.MinPrice);

        if (IsFilterSet(filter.Name))
        {
            var nameFilter = Builders<Product>.Filter.Regex(
                p => p.ProductName,
                new BsonRegularExpression($".*{filter.Name}.*", "i"));
            query = query.Where(_ => nameFilter.Inject());
        }

        FilterByCategories(ref query, filter);
        FilterBySuppliers(ref query, filter);

        ApplySorting(ref query, filter.Sort);
        int total = await query.CountAsync();

        var products = await query.Take(filter.Limit).ToListAsync();
        return new EntityFilteringResult<Product>(products, total);
    }

    public async Task<IList<Category>> GetCategoriesByProductAliasAsync(string alias)
    {
        return await DbSet.AsQueryable()
            .Where(p => p.Alias == alias)
            .GroupJoin(
                Context.GetCollection<Category>().AsQueryable(),
                p => p.CategoryId,
                c => c.CategoryId,
                (_, categoriesByProduct) => categoriesByProduct.ToList())
            .SingleAsync();
    }

    public async Task<Supplier> GetSupplierByProductAliasAsync(string alias)
    {
        return await DbSet.AsQueryable()
            .Where(p => p.Alias == alias)
            .Join(
                Context.GetCollection<Supplier>().AsQueryable(),
                p => p.SupplierId,
                s => s.SupplierId,
                (_, supplier) => supplier)
            .SingleAsync();
    }

    private static bool IsFilterSet(string? filterBy)
    {
        return !string.IsNullOrEmpty(filterBy);
    }

    private void FilterByCategories(ref IMongoQueryable<Product> query, GamesFilter filter)
    {
        if (filter.MongoCategories.Any() || filter.Genres.Any())
        {
            query = query.GroupJoin(
                    Context.GetCollection<Category>().AsQueryable(),
                    p => p.CategoryId,
                    c => c.CategoryId,
                    (product, categoriesByProduct) => new { product, categoriesByProduct })
                .Where(j => j.categoriesByProduct.Any(c => filter.MongoCategories.Contains(c.Id)))
                .Select(q => q.product);
        }
    }

    private void FilterBySuppliers(ref IMongoQueryable<Product> query, GamesFilter filter)
    {
        if (filter.MongoSuppliers.Any() || filter.Publishers.Any())
        {
            query = query.GroupJoin(
                    Context.GetCollection<Supplier>().AsQueryable(),
                    p => p.SupplierId,
                    s => s.SupplierId,
                    (product, suppliersByProduct) => new { product, suppliersByProduct })
                .Where(j => j.suppliersByProduct.Any(s => filter.MongoSuppliers.Contains(s.Id)))
                .Select(q => q.product);
        }
    }

    private static void ApplySorting(ref IMongoQueryable<Product> query, string? sorting)
    {
        if (string.IsNullOrEmpty(sorting))
        {
            return;
        }

        query = sorting switch
        {
            SortingOption.PriceAsc => query.OrderBy(g => g.UnitPrice),
            SortingOption.PriceDesc => query.OrderByDescending(g => g.UnitPrice),
            SortingOption.MostCommented => query,
            SortingOption.MostPopular => query,
            SortingOption.Newest => query,
            _ => throw new ArgumentOutOfRangeException(nameof(GamesFilterDto.Sort)),
        };
    }
}