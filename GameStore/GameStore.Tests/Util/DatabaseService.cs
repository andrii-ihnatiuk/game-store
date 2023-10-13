using GameStore.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Tests.Util;

public static class DatabaseService
{
    private const string InMemoryConnectionString = "DataSource=:memory:";

    public static GameStoreDbContext CreateSqLiteContext()
    {
        var connection = new SqliteConnection(InMemoryConnectionString);
        connection.Open();

        var options = new DbContextOptionsBuilder<GameStoreDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new GameStoreDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return context;
    }
}