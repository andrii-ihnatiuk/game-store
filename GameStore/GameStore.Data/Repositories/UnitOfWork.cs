using GameStore.Data.Entities;

namespace GameStore.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly GameStoreDbContext _context;
    private bool _disposed;

    public UnitOfWork(
        GameStoreDbContext context,
        IGenericRepository<Game> gameRepository,
        IGenericRepository<Genre> genreRepository,
        IGenericRepository<Platform> platformRepository,
        IGenericRepository<Publisher> publishersRepository,
        IGenericRepository<GameGenre> gamesGenresRepository,
        IGenericRepository<GamePlatform> gamesPlatformsRepository,
        IGenericRepository<Order> ordersRepository,
        IGenericRepository<OrderDetail> orderDetailsRepository,
        IGenericRepository<PaymentMethod> paymentMethodsRepository,
        IGenericRepository<Comment> commentsRepository)
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
    }

    public IGenericRepository<Game> Games { get; }

    public IGenericRepository<Genre> Genres { get; }

    public IGenericRepository<Platform> Platforms { get; }

    public IGenericRepository<Publisher> Publishers { get; }

    public IGenericRepository<GameGenre> GamesGenres { get; }

    public IGenericRepository<GamePlatform> GamesPlatforms { get; }

    public IGenericRepository<Order> Orders { get; }

    public IGenericRepository<OrderDetail> OrderDetails { get; }

    public IGenericRepository<PaymentMethod> PaymentMethods { get; }

    public IGenericRepository<Comment> Comments { get; }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
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