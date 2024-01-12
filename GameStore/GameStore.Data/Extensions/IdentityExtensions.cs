using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using GameStore.Shared.Constants;
using GameStore.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace GameStore.Data.Extensions;

[ExcludeFromCodeCoverage]
public static class IdentityExtensions
{
    public static async Task ThrowIfFailedAsync(this Task<IdentityResult> resultTask, string? message = null)
    {
        var result = await resultTask;
        if (!result.Succeeded)
        {
            throw new IdentityException(message ?? string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

    public static IList<Claim> GetPermissionClaims(this IEnumerable<IdentityRoleClaim<string>> roleClaims)
    {
        return roleClaims
            .Where(rc => rc.ClaimType == CustomClaimTypes.Permission)
            .GroupBy(rc => new { rc.ClaimType, rc.ClaimValue })
            .Select(rc => rc.First().ToClaim())
            .ToList();
    }

    public static bool HasPermission(this ClaimsPrincipal principal, string permission)
    {
        return principal.HasClaim(CustomClaimTypes.Permission, permission);
    }

    public static bool IsPredefinedRole(this IdentityRole role)
    {
        return Roles.AllRoles.Any(r => r.Equals(role.Name));
    }
}