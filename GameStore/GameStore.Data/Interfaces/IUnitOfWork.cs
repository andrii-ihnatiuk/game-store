using System.Data;
using GameStore.Data.Entities;
using GameStore.Data.Entities.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace GameStore.Data.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGameRepository Games { get; }

    IGenericRepository<Genre> Genres { get; }

    IGenericRepository<Platform> Platforms { get; }

    IGenericRepository<Publisher> Publishers { get; }

    IGenericRepository<GameGenre> GamesGenres { get; }

    IGenericRepository<GamePlatform> GamesPlatforms { get; }

    IOrderRepository Orders { get; }

    IGenericRepository<OrderDetail> OrderDetails { get; }

    IGenericRepository<PaymentMethod> PaymentMethods { get; }

    IGenericRepository<Comment> Comments { get; }

    IGenericRepository<AppImage> Images { get; }

    IGenericRepository<NotificationMethod> NotificationMethods { get; }

    IGenericRepository<ApplicationUserRole> UsersRoles { get; }

    Task<int> SaveAsync();

    Task<int> SaveAsync(bool logChanges);

    Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
}