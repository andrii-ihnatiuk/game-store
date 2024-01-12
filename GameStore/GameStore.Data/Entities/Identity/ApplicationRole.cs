using Microsoft.AspNetCore.Identity;

namespace GameStore.Data.Entities.Identity;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole()
    {
    }

    public ApplicationRole(string roleName)
        : base(roleName)
    {
    }

    public IList<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();

    public IList<IdentityRoleClaim<string>> RoleClaims { get; set; } = new List<IdentityRoleClaim<string>>();
}