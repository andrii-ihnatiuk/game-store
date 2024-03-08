using System.Data;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GameStore.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly GameStoreDbContext _context;
    private bool _disposed;

    public UnitOfWork(
        GameStoreDbContext context,
        IGameRepository gameRepository,
        IGenericRepository<Genre> genreRepository,
        IGenericRepository<Platform> platformRepository,
        IGenericRepository<Publisher> publishersRepository,
        IGenericRepository<GameGenre> gamesGenresRepository,
        IGenericRepository<GamePlatform> gamesPlatformsRepository,
        IOrderRepository ordersRepository,
        IGenericRepository<OrderDetail> orderDetailsRepository,
        IGenericRepository<PaymentMethod> paymentMethodsRepository,
        IGenericRepository<Comment> commentsRepository,
        IGenericRepository<AppImage> images)
    {
        _context = context;
        Games = gameRepository;
        Genres = genreRepository;
        Platforms = platformRepository;
        GamesGenres = gamesGenresRepository;
        GamesPlatforms = gamesPlatformsRepository;
        Publishers = publishersRepository;
        Orders = ordersRepository;
        OrderDetails = orderDetailsRepository;
        PaymentMethods = paymentMethodsRepository;
        Comments = commentsRepository;
        Comments = commentsRepository;
        Images = images;
    }

    public IGameRepository Games { get; }

    public IGenericRepository<Genre> Genres { get; }

    public IGenericRepository<Platform> Platforms { get; }

    public IGenericRepository<Publisher> Publishers { get; }

    public IGenericRepository<GameGenre> GamesGenres { get; }

    public IGenericRepository<GamePlatform> GamesPlatforms { get; }

    public IOrderRepository Orders { get; }

    public IGenericRepository<OrderDetail> OrderDetails { get; }

    public IGenericRepository<PaymentMethod> PaymentMethods { get; }

    public IGenericRepository<Comment> Comments { get; }

    public IGenericRepository<AppImage> Images { get; }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public Task<int> SaveAsync(bool logChanges)
    {
        _context.LogChanges = logChanges;
        return SaveAsync();
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        return _context.Database.BeginTransactionAsync(isolationLevel);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        _disposed = true;
    }
}