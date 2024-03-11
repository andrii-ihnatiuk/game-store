namespace GameStore.Shared.Options;

public class AzureServiceBusOptions
{
    public string ConnectionString { get; init; }

    public string NotificationsTopic { get; init; }
}