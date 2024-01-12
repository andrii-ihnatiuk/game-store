using System.Diagnostics.CodeAnalysis;

namespace GameStore.Shared.Exceptions;

[ExcludeFromCodeCoverage]
public class IdentityException : Exception
{
    public IdentityException(string message)
        : base(message)
    {
    }
}