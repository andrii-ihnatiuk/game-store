using GameStore.Data.Entities;

namespace GameStore.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly GameStoreDbContext _context;
    private bool _disposed;

    public UnitOfWork(
        GameStoreDbContext context,
        IGenericRepository<Game> gameRepository,
        IGenericRepository<Genre> genericRepository)
    {
        _context = context;
        Games = gameRepository;
        Genres = genericRepository;
    }

    public IGenericRepository<Game> Games { get; }

    public IGenericRepository<Genre> Genres { get; }

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