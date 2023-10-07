using GameStore.Data.Entities;

namespace GameStore.Data.Repositories;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Game> Games { get; }

    IGenericRepository<Genre> Genres { get; }

    IGenericRepository<Platform> Platforms { get; }

    Task<int> SaveAsync();
}