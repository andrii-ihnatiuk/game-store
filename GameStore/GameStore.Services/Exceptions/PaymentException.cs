using System.Diagnostics.CodeAnalysis;

namespace GameStore.Services.Exceptions;

[ExcludeFromCodeCoverage]
public class PaymentException : Exception
{
    public PaymentException(string message)
        : base(message)
    {
    }
}