using GameStore.Data.Entities.Identity;

namespace GameStore.Services.Interfaces.Authentication;

public interface IJwtProvider
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
}