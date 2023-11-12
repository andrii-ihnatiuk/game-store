using System.Diagnostics.CodeAnalysis;

namespace GameStore.Shared.Exceptions;

[ExcludeFromCodeCoverage]
public class ForeignKeyException : Exception
{
    public ForeignKeyException(string onColumn)
        : base($"The value of '{onColumn}' violates foreign key constraint on the table!")
    {
    }
}