using GameStore.Shared.Constants;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Services;

public abstract class CoreServiceBase : IResolvableByEntityStorage
{
    public EntityStorage EntityStorage => EntityStorage.SqlServer;
}