using Microsoft.EntityFrameworkCore;

namespace GameStore.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : class
{
    protected GenericRepository(GameStoreDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    protected GameStoreDbContext Context { get; }

    protected DbSet<T> DbSet { get; }

    public async Task<T> GetByIdAsync(object id)
    {
        return await DbSet.FindAsync(id);
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

        DbSet.Remove(entity);
    }

    public void Delete(object id)
    {
        T entityToDelete = DbSet.Find(id);
        if (entityToDelete != null)
        {
            Delete(entityToDelete);
        }
    }

    public void Update(T entity)
    {
        throw new NotImplementedException();
    }
}