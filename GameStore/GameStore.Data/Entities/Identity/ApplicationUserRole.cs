using Microsoft.AspNetCore.Identity;

namespace GameStore.Data.Entities.Identity;

public class ApplicationUserRole : IdentityUserRole<string>
{
    public ApplicationUser User { get; set; }

    public ApplicationRole Role { get; set; }
}