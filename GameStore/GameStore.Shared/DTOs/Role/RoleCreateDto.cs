namespace GameStore.Shared.DTOs.Role;

public class RoleCreateDto
{
    public RoleCreateInnerDto Role { get; set; }

    public IList<string> Permissions { get; set; }
}