using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace GameStore.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : class
{
    public GenericRepository(GameStoreDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    private GameStoreDbContext Context { get; }

    private DbSet<T> DbSet { get; }

    public async Task<T?> GetByIdAsync(object id)
    {
        return await DbSet.FindAsync(id);
    }

    public IQueryable<T> GetQueryable()
    {
        return DbSet;
    }

    public async Task<IEnumerable<T>> QueryAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        IQueryable<T> query = DbSet;

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (include != null)
        {
            query = include(query);
        }

        return await (orderBy != null ? orderBy(query).ToListAsync() : query.ToListAsync());
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate)
    {
        var query = await QueryAsync(predicate: predicate);
        return query.FirstOrDefault();
    }

    public async Task<IList<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    public void Delete(T entity)
    {
        if (Context.Entry(entity).State == EntityState.Detached)
        {
            DbSet.Attach(entity);
        }

        DbSet.Entry(entity).State = EntityState.Deleted;
    }

    public async Task DeleteAsync(object id)
    {
        var entityToDelete = await DbSet.FindAsync(id);
        if (entityToDelete != null)
        {
            Delete(entityToDelete);
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
}