namespace GameStore.Shared.Exceptions;

public class UserBannedException : Exception
{
    public UserBannedException(string message)
        : base(message)
    {
    }
}