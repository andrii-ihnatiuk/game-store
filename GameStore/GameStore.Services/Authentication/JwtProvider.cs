using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GameStore.Data.Entities.Identity;
using GameStore.Data.Extensions;
using GameStore.Services.Interfaces.Authentication;
using GameStore.Shared.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GameStore.Services.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly JwtOptions _jwtOptions;

    public JwtProvider(IOptions<JwtOptions> options, RoleManager<ApplicationRole> roleManager)
    {
        _jwtOptions = options.Value;
        _roleManager = roleManager;
    }

    public async Task<string> GenerateTokenAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.Name, user.UserName),
        };

        var userRoles = await _roleManager.Roles
            .Where(r => r.UserRoles.Any(ur => ur.UserId.Equals(user.Id)))
            .Include(r => r.RoleClaims)
            .ToListAsync();

        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
        var userPermissions = userRoles.SelectMany(r => r.RoleClaims);
        claims.AddRange(userPermissions.GetPermissionClaims());

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: signingCredentials);

        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return $"Bearer {tokenValue}";
    }
}