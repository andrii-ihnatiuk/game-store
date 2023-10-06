using GameStore.Data.Entities;

namespace GameStore.Data.Repositories;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Game> Games { get; }

    Task<int> SaveAsync();
}