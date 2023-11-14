using GameStore.Shared.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Northwind.Data.Interfaces;

namespace Northwind.Data;

public class MongoContext : IMongoContext
{
    private readonly IList<Func<Task>> _commands;

    public MongoContext(IOptions<MongoDbSettings> dbOptions)
    {
        var dbSettings = dbOptions.Value;
        Client = new MongoClient(dbSettings.ConnectionString);
        Database = Client.GetDatabase(dbSettings.DatabaseName);
        _commands = new List<Func<Task>>();
    }

    private IMongoDatabase Database { get; }

    private MongoClient Client { get; }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return Database.GetCollection<T>(name);
    }

    public void AddCommand(Func<Task> command)
    {
        _commands.Add(command);
    }

    public async Task<bool> SaveChangesAsync()
    {
        using var session = await Client.StartSessionAsync();
        session.StartTransaction();
        try
        {
            var tasks = _commands.Select(c => c.Invoke());
            await Task.WhenAll(tasks);
            await session.CommitTransactionAsync();
            _commands.Clear();
            return true;
        }
        catch (Exception)
        {
            await session.AbortTransactionAsync();
            return false;
        }
    }
}