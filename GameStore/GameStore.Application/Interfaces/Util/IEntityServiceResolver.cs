﻿using GameStore.Shared.Constants;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Application.Interfaces.Util;

public interface IEntityServiceResolver
{
    T ResolveForEntityId<T>(string id)
        where T : IResolvableByEntityStorage;

    T ResolveForEntityAlias<T>(string alias)
        where T : IResolvableByEntityStorage;

    T ResolveForEntityStorage<T>(EntityStorage storage)
        where T : IResolvableByEntityStorage;

    IEnumerable<T> ResolveAll<T>();
}