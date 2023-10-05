namespace GameStore.Shared.Exceptions;

public class EntityNotFoundException : Exception
{
    private const string DefaultMessage = "Entity with provided ID does not exist!";

    public EntityNotFoundException()
        : base(DefaultMessage)
    {
    }

    public EntityNotFoundException(object entityId)
        : base($"Requested entity with ID: '{entityId}' does not exist!")
    {
        EntityId = entityId;
    }

    public object EntityId { get; }
}