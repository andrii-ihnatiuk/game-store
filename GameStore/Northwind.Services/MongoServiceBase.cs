using System.Diagnostics.CodeAnalysis;
using GameStore.Shared.Constants;
using GameStore.Shared.Interfaces.Services;

namespace Northwind.Services;

[ExcludeFromCodeCoverage]
public abstract class MongoServiceBase : IResolvableByEntityStorage
{
    public EntityStorage EntityStorage => EntityStorage.MongoDb;
}