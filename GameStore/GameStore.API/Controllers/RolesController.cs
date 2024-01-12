using GameStore.API.Attributes;
using GameStore.Services.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Role;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HasAnyPermission(PermissionOptions.RoleView, PermissionOptions.RoleFull)]
    [HttpGet]
    public async Task<ActionResult<IList<RoleBriefDto>>> GetAllRolesAsync()
    {
        var roles = await _roleService.GetAllRolesAsync();
        return Ok(roles);
    }

    [HasAnyPermission(PermissionOptions.RoleView, PermissionOptions.RoleFull)]
    [HttpGet("{roleId}")]
    public async Task<ActionResult<RoleBriefDto>> GetRoleAsync([FromRoute] string roleId)
    {
        var role = await _roleService.GetRoleByIdAsync(roleId);
        return Ok(role);
    }

    [HasAnyPermission(PermissionOptions.RoleView, PermissionOptions.RoleFull)]
    [HttpGet("{roleId}/permissions")]
    public async Task<ActionResult<IList<string>>> GetRolePermissionsAsync([FromRoute] string roleId)
    {
        var permissions = await _roleService.GetRolePermissionsAsync(roleId);
        return Ok(permissions);
    }

    [HasAnyPermission(PermissionOptions.RoleCreate, PermissionOptions.RoleUpdate, PermissionOptions.RoleFull)]
    [HttpGet]
    [Route("/permissions")]
    public IActionResult GetAllPermissions()
    {
        return Ok(PermissionOptions.AllOptions);
    }

    [HasAnyPermission(PermissionOptions.RoleCreate, PermissionOptions.RoleFull)]
    [HttpPost("new")]
    public async Task<ActionResult<RoleBriefDto>> CreateRoleAsync([FromBody] RoleCreateDto dto)
    {
        var createdRole = await _roleService.CreateRoleAsync(dto);
        return Ok(createdRole);
    }

    [HasAnyPermission(PermissionOptions.RoleUpdate, PermissionOptions.RoleFull)]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateRoleAsync([FromBody] RoleUpdateDto dto)
    {
        await _roleService.UpdateRoleAsync(dto);
        return NoContent();
    }

    [HasAnyPermission(PermissionOptions.RoleDelete, PermissionOptions.RoleFull)]
    [HttpDelete("remove/{roleId}")]
    public async Task<IActionResult> DeleteRoleAsync([FromRoute] string roleId)
    {
        await _roleService.DeleteRoleAsync(roleId);
        return NoContent();
    }
}