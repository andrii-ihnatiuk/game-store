using System.Diagnostics.CodeAnalysis;

namespace GameStore.Application.Exceptions;

[ExcludeFromCodeCoverage]
public class PaymentException : Exception
{
    public PaymentException(string message)
        : base(message)
    {
    }
}