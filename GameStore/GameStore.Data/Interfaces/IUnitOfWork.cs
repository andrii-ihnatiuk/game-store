using GameStore.Data.Entities;

namespace GameStore.Data.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGameRepository Games { get; }

    IGenericRepository<Genre> Genres { get; }

    IGenericRepository<Platform> Platforms { get; }

    IGenericRepository<Publisher> Publishers { get; }

    IGenericRepository<GameGenre> GamesGenres { get; }

    IGenericRepository<GamePlatform> GamesPlatforms { get; }

    IGenericRepository<Order> Orders { get; }

    IGenericRepository<OrderDetail> OrderDetails { get; }

    IGenericRepository<PaymentMethod> PaymentMethods { get; }

    IGenericRepository<Comment> Comments { get; }

    Task<int> SaveAsync();
}