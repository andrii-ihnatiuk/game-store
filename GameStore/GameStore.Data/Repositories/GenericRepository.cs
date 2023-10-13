using System.Linq.Expressions;
using GameStore.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace GameStore.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : class
{
    public GenericRepository(GameStoreDbContext context)
    {
        DbSet = context.Set<T>();
    }

    private DbSet<T> DbSet { get; }

    public async Task<T> GetByIdAsync(object id)
    {
        var entity = await DbSet.FindAsync(id);
        return entity ?? throw new EntityNotFoundException(entityId: id);
    }

    public async Task<IList<T>> GetAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        bool noTracking = true)
    {
        IQueryable<T> query = DbSet;
        query = noTracking ? query.AsNoTracking() : query;
        query = predicate != null ? query.Where(predicate) : query;
        query = include != null ? include(query) : query;

        return await (orderBy != null ? orderBy(query).ToListAsync() : query.ToListAsync());
    }

    public async Task<T> GetOneAsync(
        Expression<Func<T, bool>>? predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        bool noTracking = true)
    {
        var query = await GetAsync(predicate: predicate, include: include, noTracking: noTracking);
        var entity = query.FirstOrDefault();
        return entity ?? throw new EntityNotFoundException();
    }

    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task DeleteAsync(object id)
    {
        var entityToDelete = await DbSet.FindAsync(id);
        if (entityToDelete != null)
        {
            DbSet.Remove(entityToDelete);
        }
    }

    public void Update(T entity)
    {
        if (DbSet.Entry(entity).State == EntityState.Detached)
        {
            DbSet.Attach(entity);
        }

        DbSet.Entry(entity).State = EntityState.Modified;
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> condition)
    {
        return await DbSet.AnyAsync(condition);
    }
}