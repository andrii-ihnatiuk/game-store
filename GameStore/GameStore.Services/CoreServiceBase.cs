using System.Diagnostics.CodeAnalysis;
using GameStore.Shared.Constants;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Services;

[ExcludeFromCodeCoverage]
public abstract class CoreServiceBase : IResolvableByEntityStorage
{
    public EntityStorage EntityStorage => EntityStorage.SqlServer;
}