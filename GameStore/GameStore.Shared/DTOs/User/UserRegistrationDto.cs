namespace GameStore.Shared.DTOs.User;

public class UserRegistrationDto
{
    public UserRegistrationInnerDto User { get; set; }

    public string Password { get; set; }

    public IList<string> Roles { get; set; }
}