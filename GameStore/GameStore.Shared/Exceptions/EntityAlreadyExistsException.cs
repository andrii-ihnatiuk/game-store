namespace GameStore.Shared.Exceptions;

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