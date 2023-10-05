namespace GameStore.Data.Repositories;

public interface IGenericRepository<T>
    where T : class
{
    Task<T> GetByIdAsync(object id);

    Task<IList<T>> GetAllAsync();

    Task AddAsync(T entity);

    void Delete(T entity);

    void Delete(object id);

    void Update(T entity);
}