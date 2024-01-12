namespace GameStore.Shared.DTOs.Authentication;

public class ExternalLoginRequestDto
{
    public ExternalLoginRequestDto(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; set; }

    public string Password { get; set; }
}