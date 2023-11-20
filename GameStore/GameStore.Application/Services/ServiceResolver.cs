using System.Text.RegularExpressions;
using GameStore.Application.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Data.Serializers;

namespace GameStore.Application.Services;

public class ServiceResolver : IServiceResolver
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T ResolveForEntityId<T>(string id)
        where T : IResolvableByEntityStorage
    {
        var storage = GetEntityStorageForIdString(id);
        var services = _serviceProvider.GetServices<T>();
        return services.Single(s => s.EntityStorage == storage);
    }

    public T ResolveForEntityAlias<T>(string alias)
        where T : IResolvableByEntityStorage
    {
        var services = _serviceProvider.GetServices<T>();
        return Regex.IsMatch(alias, $".*{EntityAliasSerializer.AliasSuffix}$")
            ? services.Single(s => s.EntityStorage == EntityStorage.MongoDb)
            : services.Single(s => s.EntityStorage == EntityStorage.SqlServer);
    }

    public IEnumerable<T> ResolveAll<T>()
    {
        return _serviceProvider.GetServices<T>();
    }

    private static EntityStorage GetEntityStorageForIdString(string id)
    {
        EntityStorage type;
        if (Guid.TryParse(id, out _))
        {
            type = EntityStorage.SqlServer;
        }
        else
        {
            type = EntityStorage.MongoDb;
        }

        return type;
    }
}