using GameStore.Data.Entities;

namespace GameStore.Data.Repositories;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Game> Games { get; }

    IGenericRepository<Genre> Genres { get; }

    IGenericRepository<Platform> Platforms { get; }

    IGenericRepository<Publisher> Publishers { get; }

    IGenericRepository<GameGenre> GamesGenres { get; }

    IGenericRepository<GamePlatform> GamesPlatforms { get; }

    Task<int> SaveAsync();
}