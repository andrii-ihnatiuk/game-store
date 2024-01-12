using GameStore.Data.Entities.Identity;
using GameStore.Services.Interfaces.Authentication;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.User;
using GameStore.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace GameStore.Services.Authentication;

public class InternalLoginService : ILoginService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtProvider _jwtProvider;

    public InternalLoginService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
    }

    public LoginMethod LoginMethod => LoginMethod.Internal;

    public async Task<string> LoginAsync(UserLoginDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.Model.Login);
        if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Model.Password))
        {
            throw new LoginException();
        }

        return await _jwtProvider.GenerateTokenAsync(user);
    }
}