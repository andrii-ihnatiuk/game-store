using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace GameStore.Data.Repositories;

public interface IGenericRepository<T>
    where T : class
{
    Task<T> GetByIdAsync(object id);

    Task<IList<T>> GetAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        bool noTracking = true);

    Task<T> GetOneAsync(
        Expression<Func<T, bool>>? predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        bool noTracking = true);

    Task AddAsync(T entity);

    void Delete(T entity);

    Task DeleteAsync(object id);

    void Update(T entity);

    Task<bool> ExistsAsync(Expression<Func<T, bool>> condition);
}