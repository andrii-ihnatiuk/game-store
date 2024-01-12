using GameStore.Shared.DTOs.Role;
using GameStore.Shared.DTOs.User;

namespace GameStore.Services.Interfaces;

public interface IUserService
{
    Task<IList<UserBriefDto>> GetAllUsersAsync();

    Task<UserBriefDto> GetUserByIdAsync(string userId);

    Task<IList<RoleBriefDto>> GetRolesByUserAsync(string userId);

    Task<UserBriefDto> RegisterUserAsync(UserRegistrationDto dto);

    Task UpdateUserAsync(UserUpdateDto dto);

    Task DeleteUserAsync(string userId);
}