using System.Diagnostics.CodeAnalysis;

namespace GameStore.Shared.Exceptions;

[ExcludeFromCodeCoverage]
public class OrderFromNorthwindException : Exception
{
    private const string DefaultMessage = "Ordering items from Northwind is not supported.";

    public OrderFromNorthwindException()
        : base(DefaultMessage)
    {
    }
}