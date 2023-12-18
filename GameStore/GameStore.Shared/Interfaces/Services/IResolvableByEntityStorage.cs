using GameStore.Shared.Constants;

namespace GameStore.Shared.Interfaces.Services;

public interface IResolvableByEntityStorage
{
    EntityStorage EntityStorage { get; }
}