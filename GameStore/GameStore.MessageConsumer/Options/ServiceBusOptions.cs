namespace GameStore.MessageConsumer.Options;

public class ServiceBusOptions
{
    public string ConnectionString { get; init; }

    public string TopicName { get; init; }

    public string EmailSubscriptionName { get; init; }

    public string SmsSubscriptionName { get; init; }

    public string PushSubscriptionName { get; init; }
}