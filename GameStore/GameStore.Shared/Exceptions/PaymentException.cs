using System.Diagnostics.CodeAnalysis;

namespace GameStore.Shared.Exceptions;

[ExcludeFromCodeCoverage]
public class PaymentException : Exception
{
    public PaymentException(string message)
        : base(message)
    {
    }
}