using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using GameStore.Shared.Exceptions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Northwind.Data.Entities;
using Northwind.Data.Interfaces;

namespace Northwind.Data.Repositories;

[ExcludeFromCodeCoverage]
public class GenericRepository<T> : IGenericRepository<T>
    where T : BaseEntity
{
    public GenericRepository(IMongoContext context)
    {
        Context = context;
        DbSet = context.GetCollection<T>();
    }

    protected IMongoContext Context { get; }

    protected IMongoCollection<T> DbSet { get; }

    public async Task<T> GetOneAsync(Expression<Func<T, bool>> predicate)
    {
        var filter = GetFilterDefinition(predicate);
        var query = await DbSet.FindAsync(filter);
        var entity = query.FirstOrDefault();
        return entity ?? throw new EntityNotFoundException();
    }

    public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate)
    {
        var filter = GetFilterDefinition(predicate);
        var entities = await DbSet.Find(filter).ToCursorAsync();
        return entities.ToList();
    }

    public Task AddAsync(T entity)
    {
        Context.AddCommand(session => DbSet.InsertOneAsync(session, entity));
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.AsQueryable().AnyAsync(predicate);
    }

    public Task UpdateAsync(T entity)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, entity.Id);
        Context.AddCommand(session => DbSet.ReplaceOneAsync(session, filter, entity));
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string id)
    {
        Context.AddCommand(session => DbSet.DeleteOneAsync(session, e => e.Id == id));
        return Task.CompletedTask;
    }

    private static FilterDefinition<T> GetFilterDefinition(Expression<Func<T, bool>>? predicate)
    {
        return predicate ?? FilterDefinition<T>.Empty;
    }
}