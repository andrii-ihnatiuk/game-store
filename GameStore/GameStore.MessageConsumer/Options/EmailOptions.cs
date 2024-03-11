namespace GameStore.MessageConsumer.Options;

public class EmailOptions
{
    public string Host { get; init; }

    public int Port { get; init; }

    public string Security { get; init; }

    public string Username { get; init; }

    public string Password { get; init; }

    public string SenderEmail { get; init; }

    public string SenderName { get; init; }
}