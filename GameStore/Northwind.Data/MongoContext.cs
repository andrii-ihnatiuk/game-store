﻿using System.Diagnostics.CodeAnalysis;
using GameStore.Shared.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Northwind.Data.Attributes;
using Northwind.Data.Interfaces;

namespace Northwind.Data;

[ExcludeFromCodeCoverage]
public class MongoContext : IMongoContext
{
    private readonly IList<Func<IClientSessionHandle, Task>> _commands;

    public MongoContext(IOptions<MongoDbOptions> dbOptions)
    {
        var dbSettings = dbOptions.Value;
        Client = new MongoClient(dbSettings.ConnectionString);
        Database = Client.GetDatabase(dbSettings.DatabaseName);
        _commands = new List<Func<IClientSessionHandle, Task>>();
    }

    private IMongoDatabase Database { get; }

    private MongoClient Client { get; }

    public IMongoCollection<T> GetCollection<T>(string? name)
    {
        string collectionName = name ?? GetCollectionNameFromType(typeof(T));
        return Database.GetCollection<T>(collectionName);
    }

    public void AddCommand(Func<IClientSessionHandle, Task> command)
    {
        _commands.Add(command);
    }

    public async Task<bool> SaveChangesAsync()
    {
        using var session = await Client.StartSessionAsync();

        // standalone instances do not support transactions
        // session.StartTransaction();
        try
        {
            var tasks = _commands.Select(c => c.Invoke(session));
            await Task.WhenAll(tasks);

            // await session.CommitTransactionAsync();
            _commands.Clear();
            return true;
        }
        catch (Exception)
        {
            // await session.AbortTransactionAsync();
            return false;
        }
    }

    private static string GetCollectionNameFromType(Type type)
    {
        object? attribute = type.GetCustomAttributes(typeof(BsonCollectionAttribute), inherit: false).FirstOrDefault();
        string? name = (attribute as BsonCollectionAttribute)?.CollectionName;
        return name ?? type.Name;
    }
}