using System.Diagnostics.CodeAnalysis;

namespace GameStore.Data.Exceptions;

[ExcludeFromCodeCoverage]
public class EntityAlreadyExistsException : Exception
{
    private const string DefaultMessage = "Entity already exists!";

    public EntityAlreadyExistsException()
        : base(DefaultMessage)
    {
    }

    public EntityAlreadyExistsException(string fieldName, object value)
        : base($"Entity with '{fieldName}': '{value}' already exists!")
    {
    }
}