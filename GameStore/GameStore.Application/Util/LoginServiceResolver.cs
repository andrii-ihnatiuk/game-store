using GameStore.Application.Interfaces.Util;
using GameStore.Services.Interfaces.Authentication;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.User;

namespace GameStore.Application.Util;

public class LoginServiceResolver : ILoginServiceResolver
{
    private readonly IServiceProviderWrapper _serviceProvider;

    public LoginServiceResolver(IServiceProviderWrapper serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ILoginService Resolve(UserLoginModel model)
    {
        var services = _serviceProvider.GetServices<ILoginService>();
        return model.InternalAuth
            ? services.Single(s => s.LoginMethod.Equals(LoginMethod.Internal))
            : services.Single(s => s.LoginMethod.Equals(LoginMethod.AuthApi));
    }
}