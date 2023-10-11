namespace GameStore.Data.Exceptions;

public class ForeignKeyException : Exception
{
    public ForeignKeyException(string onColumn)
        : base($"The value of '{onColumn}' violates foreign key constraint on the table!")
    {
    }
}