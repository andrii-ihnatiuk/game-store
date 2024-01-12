using GameStore.Shared.DTOs.User;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Services.Interfaces.Authentication;

public interface ILoginService : IResolvableByLoginMethod
{
    Task<string> LoginAsync(UserLoginDto dto);
}