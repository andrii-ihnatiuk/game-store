using System.Diagnostics.CodeAnalysis;

namespace GameStore.Shared.Exceptions;

[ExcludeFromCodeCoverage]
public class GameStoreNotSupportedException : Exception
{
    private const string DefaultMessage = "Sorry, this operation is not supported.";

    public GameStoreNotSupportedException()
        : base(DefaultMessage)
    {
    }

    public GameStoreNotSupportedException(string message)
        : base(message)
    {
    }
}