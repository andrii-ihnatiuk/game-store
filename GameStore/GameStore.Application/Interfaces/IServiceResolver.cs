using GameStore.Shared.Interfaces.Services;

namespace GameStore.Application.Interfaces;

public interface IServiceResolver
{
    T ResolveForEntityId<T>(string id)
        where T : IResolvableByEntityStorage;

    T ResolveForEntityAlias<T>(string alias)
        where T : IResolvableByEntityStorage;

    IEnumerable<T> ResolveAll<T>();
}