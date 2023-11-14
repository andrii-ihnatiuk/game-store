using MongoDB.Driver;

namespace Northwind.Data.Interfaces;

public interface IMongoContext
{
    IMongoCollection<T> GetCollection<T>(string? name = null);

    void AddCommand(Func<Task> command);

    Task<bool> SaveChangesAsync();
}