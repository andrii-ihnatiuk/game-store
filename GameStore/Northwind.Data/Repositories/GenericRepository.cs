using System.Linq.Expressions;
using MongoDB.Driver;
using Northwind.Data.Attributes;
using Northwind.Data.Interfaces;

namespace Northwind.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : class
{
    private readonly IMongoContext _context;

    public GenericRepository(IMongoContext context)
    {
        _context = context;
        DbSet = _context.GetCollection<T>(GetCollectionName());
    }

    protected IMongoCollection<T> DbSet { get; }

    public async Task<T> GetByIdAsync(string id)
    {
        var entity = await DbSet.FindAsync(Builders<T>.Filter.Eq("_id", id));
        return entity.FirstOrDefault();
    }

    public async Task<T> GetOneAsync(Expression<Func<T, bool>>? predicate)
    {
        var filter = GetFilterDefinition(predicate);
        var entity = await DbSet.FindAsync(filter);
        return entity.FirstOrDefault();
    }

    public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate)
    {
        var filter = GetFilterDefinition(predicate);
        var entities = await DbSet.Find(filter).ToCursorAsync();
        return entities.ToList();
    }

    private static string GetCollectionName()
    {
        var type = typeof(T);
        object? attribute = type.GetCustomAttributes(typeof(BsonCollectionAttribute), inherit: false).FirstOrDefault();
        string? name = (attribute as BsonCollectionAttribute)?.CollectionName;
        return name ?? type.Name;
    }

    private static FilterDefinition<T> GetFilterDefinition(Expression<Func<T, bool>>? predicate)
    {
        return predicate ?? FilterDefinition<T>.Empty;
    }
}