using GameStore.Shared.Constants;

namespace GameStore.Shared.Interfaces.Services;

public interface IResolvableByLoginMethod
{
    LoginMethod LoginMethod { get; }
}