namespace GameStore.Services.Exceptions;

public class UserBannedException : Exception
{
    public UserBannedException(string message)
        : base(message)
    {
    }
}