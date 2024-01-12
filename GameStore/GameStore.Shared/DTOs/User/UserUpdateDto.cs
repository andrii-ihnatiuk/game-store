namespace GameStore.Shared.DTOs.User;

public class UserUpdateDto
{
    public UserUpdateInnerDto User { get; set; }

    public string Password { get; set; }

    public IList<string> Roles { get; set; }
}