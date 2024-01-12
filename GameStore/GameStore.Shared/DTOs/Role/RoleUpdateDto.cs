namespace GameStore.Shared.DTOs.Role;

public class RoleUpdateDto
{
    public RoleUpdateInnerDto Role { get; set; }

    public IList<string> Permissions { get; set; }
}