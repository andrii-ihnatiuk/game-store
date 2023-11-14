using System.Linq.Expressions;

namespace Northwind.Data.Interfaces;

public interface IGenericRepository<T>
    where T : class
{
    Task<T> GetOneAsync(Expression<Func<T, bool>> predicate);

    Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate);
}