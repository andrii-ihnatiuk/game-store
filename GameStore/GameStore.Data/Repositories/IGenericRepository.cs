using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace GameStore.Data.Repositories;

public interface IGenericRepository<T>
    where T : class
{
    Task<T?> GetByIdAsync(object id);

    IQueryable<T> GetQueryable();

    Task<IList<T>> GetAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        bool noTracking = true);

    Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>>? predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        bool noTracking = true);

    Task AddAsync(T entity);

    void Delete(T entity);

    Task DeleteAsync(object id);

    void Update(T entity);
}