namespace GameStore.Shared.DTOs.User;

public class UserLoginModel
{
    public string Login { get; set; }

    public string Password { get; set; }

    public bool InternalAuth { get; set; }
}