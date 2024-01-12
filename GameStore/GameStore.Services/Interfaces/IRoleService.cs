using GameStore.Shared.DTOs.Role;

namespace GameStore.Services.Interfaces;

public interface IRoleService
{
    Task<IList<RoleBriefDto>> GetAllRolesAsync();

    Task<RoleBriefDto> GetRoleByIdAsync(string roleId);

    Task<IList<string>> GetRolePermissionsAsync(string roleId);

    Task<RoleBriefDto> CreateRoleAsync(RoleCreateDto roleDto);

    Task UpdateRoleAsync(RoleUpdateDto roleDto);

    Task DeleteRoleAsync(string roleId);
}