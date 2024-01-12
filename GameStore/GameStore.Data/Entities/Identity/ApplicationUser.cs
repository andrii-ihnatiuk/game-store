using Microsoft.AspNetCore.Identity;

namespace GameStore.Data.Entities.Identity;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
    }

    public ApplicationUser(string userName)
        : base(userName)
    {
    }

    public IList<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
}