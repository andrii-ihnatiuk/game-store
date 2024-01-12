using GameStore.Services.Interfaces.Authentication;
using GameStore.Shared.DTOs.User;

namespace GameStore.Application.Interfaces.Util;

public interface ILoginServiceResolver
{
    ILoginService Resolve(UserLoginModel model);
}