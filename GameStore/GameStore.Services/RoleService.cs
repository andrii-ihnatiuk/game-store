using System.Security.Claims;
using System.Transactions;
using AutoMapper;
using GameStore.Data.Entities.Identity;
using GameStore.Data.Extensions;
using GameStore.Services.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Role;
using GameStore.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IMapper _mapper;

    public RoleService(RoleManager<ApplicationRole> roleManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<IList<RoleBriefDto>> GetAllRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return _mapper.Map<IList<RoleBriefDto>>(roles);
    }

    public async Task<RoleBriefDto> GetRoleByIdAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        ThrowIfRoleWithIdNotFound(role, roleId);
        return _mapper.Map<RoleBriefDto>(role);
    }

    public async Task<IList<string>> GetRolePermissionsAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        ThrowIfRoleWithIdNotFound(role, roleId);
        var roleClaims = await _roleManager.GetClaimsAsync(role);
        return roleClaims.Where(c => c.Type == CustomClaimTypes.Permission).Select(c => c.Value).ToList();
    }

    public async Task<RoleBriefDto> CreateRoleAsync(RoleCreateDto roleDto)
    {
        var role = new ApplicationRole(roleDto.Role.Name);

        var transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
        using var scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled);

        await _roleManager.CreateAsync(role).ThrowIfFailedAsync();
        var claims = CreateClaimsFromPermissions(roleDto.Permissions);
        await AddClaimsToRoleAsync(role, claims);

        scope.Complete();
        return _mapper.Map<RoleBriefDto>(role);
    }

    public async Task UpdateRoleAsync(RoleUpdateDto roleDto)
    {
        var role = await _roleManager.Roles
            .Where(r => r.Id.Equals(roleDto.Role.Id))
            .Include(r => r.RoleClaims)
            .SingleOrDefaultAsync();

        ThrowIfRoleWithIdNotFound(role, roleDto.Role.Id);

        if (role!.IsPredefinedRole() && role.Name != roleDto.Role.Name)
        {
            throw new GameStoreNotSupportedException("The name of predefined role cannot be changed!");
        }

        var roleToUpdate = _mapper.Map(roleDto, role!);
        var transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
        using var scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled);

        await _roleManager.UpdateAsync(roleToUpdate).ThrowIfFailedAsync();
        var claims = roleToUpdate.RoleClaims.GetPermissionClaims();
        await RemoveClaimsFromRoleAsync(roleToUpdate, claims);
        claims = CreateClaimsFromPermissions(roleDto.Permissions);
        await AddClaimsToRoleAsync(roleToUpdate, claims);

        scope.Complete();
    }

    public async Task DeleteRoleAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        ThrowIfRoleWithIdNotFound(role, roleId);
        if (role.IsPredefinedRole())
        {
            throw new GameStoreNotSupportedException("Cannot delete predefined role!");
        }

        await _roleManager.DeleteAsync(role).ThrowIfFailedAsync();
    }

    private static void ThrowIfRoleWithIdNotFound(ApplicationRole? role, string id)
    {
        if (role is null)
        {
            throw new EntityNotFoundException(id);
        }
    }

    private static IList<Claim> CreateClaimsFromPermissions(IEnumerable<string> permissions)
    {
        permissions = permissions.ToList();
        IList<Claim> claims = new List<Claim>(permissions.Count());

        foreach (string permission in permissions)
        {
            claims.Add(new Claim(CustomClaimTypes.Permission, permission));
        }

        return claims;
    }

    private async Task AddClaimsToRoleAsync(ApplicationRole role, IEnumerable<Claim> claims)
    {
        foreach (var claim in claims)
        {
            await _roleManager.AddClaimAsync(role, claim).ThrowIfFailedAsync();
        }
    }

    private async Task RemoveClaimsFromRoleAsync(ApplicationRole role, IEnumerable<Claim> claims)
    {
        foreach (var claim in claims)
        {
            await _roleManager.RemoveClaimAsync(role, claim).ThrowIfFailedAsync();
        }
    }
}