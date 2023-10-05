namespace GameStore.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly GameStoreDbContext _context;
    private bool _disposed;

    public UnitOfWork(GameStoreDbContext context, IGameRepository gameRepository)
    {
        _context = context;
        Games = gameRepository;
    }

    public IGameRepository Games { get; }

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