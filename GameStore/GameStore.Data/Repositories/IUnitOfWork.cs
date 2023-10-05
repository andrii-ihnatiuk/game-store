namespace GameStore.Data.Repositories;

public interface IUnitOfWork : IDisposable
{
    IGameRepository Games { get; }

    Task<int> SaveAsync();
}