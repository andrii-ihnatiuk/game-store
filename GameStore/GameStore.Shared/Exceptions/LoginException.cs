using System.Diagnostics.CodeAnalysis;

namespace GameStore.Shared.Exceptions;

[ExcludeFromCodeCoverage]
public class LoginException : Exception
{
    private const string DefaultMessage = "Either the login or password is incorrect.";

    public LoginException()
        : base(DefaultMessage)
    {
    }

    public LoginException(string message)
        : base(message)
    {
    }
}