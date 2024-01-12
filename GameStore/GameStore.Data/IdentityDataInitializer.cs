using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Transactions;
using GameStore.Data.Entities.Identity;
using GameStore.Data.Extensions;
using GameStore.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Data;

[ExcludeFromCodeCoverage]
public static class IdentityDataInitializer
{
    private const string AdminRoleName = "Administrator";
    private const string ManagerRoleName = "Manager";
    private const string ModeratorRoleName = "Moderator";
    private const string PublisherRoleName = "Publisher";
    private const string UserRoleName = "User";

    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
        using var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled);

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        SeedRolesWithPermissionsAsync(roleManager).GetAwaiter().GetResult();
        SeedUsersWithRolesAsync(userManager).GetAwaiter().GetResult();

        transactionScope.Complete();
    }

    private static async Task SeedRolesWithPermissionsAsync(RoleManager<ApplicationRole> roleManager)
    {
        var rolesClaims = new Dictionary<string, IEnumerable<Claim>>()
        {
            {
                AdminRoleName, new Claim[]
                {
                    new(CustomClaimTypes.Permission, PermissionOptions.UserFull),
                    new(CustomClaimTypes.Permission, PermissionOptions.RoleFull),
                    new(CustomClaimTypes.Permission, PermissionOptions.GameFull),
                    new(CustomClaimTypes.Permission, PermissionOptions.GenreFull),
                    new(CustomClaimTypes.Permission, PermissionOptions.PublisherFull),
                    new(CustomClaimTypes.Permission, PermissionOptions.PlatformFull),
                    new(CustomClaimTypes.Permission, PermissionOptions.CommentFull),
                    new(CustomClaimTypes.Permission, PermissionOptions.OrderFull),
                }
            },
            {
                ManagerRoleName, new Claim[]
                {
                    new(CustomClaimTypes.Permission, PermissionOptions.GameViewDeleted),
                    new(CustomClaimTypes.Permission, PermissionOptions.GameCreate),
                    new(CustomClaimTypes.Permission, PermissionOptions.GameUpdate),
                    new(CustomClaimTypes.Permission, PermissionOptions.GameDelete),
                    new(CustomClaimTypes.Permission, PermissionOptions.GenreFull),
                    new(CustomClaimTypes.Permission, PermissionOptions.PublisherFull),
                    new(CustomClaimTypes.Permission, PermissionOptions.PlatformFull),
                    new(CustomClaimTypes.Permission, PermissionOptions.OrderViewHistory),
                    new(CustomClaimTypes.Permission, PermissionOptions.OrderUpdate),
                }
            },
            {
                ModeratorRoleName, new Claim[]
                {
                    new(CustomClaimTypes.Permission, PermissionOptions.GameViewDeleted),
                    new(CustomClaimTypes.Permission, PermissionOptions.CommentUpdate),
                    new(CustomClaimTypes.Permission, PermissionOptions.CommentBan),
                    new(CustomClaimTypes.Permission, PermissionOptions.CommentDelete),
                }
            },
            {
                PublisherRoleName, new Claim[]
                {
                    new(CustomClaimTypes.Permission, PermissionOptions.PublisherUpdateSelf),
                    new(CustomClaimTypes.Permission, PermissionOptions.GameUpdateOwn),
                }
            },
            {
                UserRoleName, Array.Empty<Claim>()
            },
        };

        foreach (string roleName in rolesClaims.Keys)
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                continue;
            }

            var role = new ApplicationRole(roleName);
            await roleManager.CreateAsync(role).ThrowIfFailedAsync();

            foreach (var claim in rolesClaims[roleName])
            {
                await roleManager.AddClaimAsync(role, claim).ThrowIfFailedAsync();
            }
        }
    }

    private static async Task SeedUsersWithRolesAsync(UserManager<ApplicationUser> userManager)
    {
        var users = new UserInfo[]
        {
            new()
            {
                User = new ApplicationUser("Admin"),
                Password = "test",
                Roles = new[] { AdminRoleName },
            },
        };

        foreach (var userInfo in users)
        {
            if (await userManager.Users.AnyAsync(u => u.UserName.Equals(userInfo.User.UserName)))
            {
                continue;
            }

            await userManager.CreateAsync(userInfo.User, userInfo.Password).ThrowIfFailedAsync();
            await userManager.AddToRolesAsync(userInfo.User, userInfo.Roles).ThrowIfFailedAsync();
        }
    }

    private record struct UserInfo(ApplicationUser User, string Password, string[] Roles);
}