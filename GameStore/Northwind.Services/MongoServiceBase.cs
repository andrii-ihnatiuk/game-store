using GameStore.Shared.Constants;
using GameStore.Shared.Interfaces.Services;

namespace Northwind.Services;

public abstract class MongoServiceBase : IResolvableByEntityStorage
{
    public EntityStorage EntityStorage => EntityStorage.MongoDb;
}