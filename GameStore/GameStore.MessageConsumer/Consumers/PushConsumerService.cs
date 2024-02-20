using Azure.Messaging.ServiceBus;
using GameStore.MessageConsumer.Interfaces;
using GameStore.MessageConsumer.Messages.Order;
using GameStore.MessageConsumer.Options;
using GameStore.MessageConsumer.Util;
using GameStore.Messaging.DTOs.Order;
using Microsoft.Extensions.Options;

namespace GameStore.MessageConsumer.Consumers;

public class PushConsumerService : AzureTopicConsumerBase
{
    private readonly ILogger<PushConsumerService> _logger;
    private readonly ServiceBusOptions _serviceBusOptions;

    public PushConsumerService(
        IOptions<ServiceBusOptions> serviceBusOptions,
        ILogger<PushConsumerService> logger,
        IServiceScopeFactory scopeFactory)
        : base(logger, scopeFactory)
    {
        _logger = logger;
        _serviceBusOptions = serviceBusOptions.Value;
    }

    protected override string ConnectionString => _serviceBusOptions.ConnectionString;

    protected override string TopicName => _serviceBusOptions.TopicName;

    protected override string SubscriptionName => _serviceBusOptions.PushSubscriptionName;

    protected override Task ProcessMessageInternalAsync(ServiceBusReceivedMessage message, IServiceProvider provider)
    {
        _logger.LogInformation("Processing push message with body:\n{body}", message.Body);

        IPushMessage pushMessage = ConsumerUtil.GetMessageTypeName(message) switch
        {
            nameof(OrderStatusMessageDto) => message.Body.ToObjectFromJson<OrderStatusMessage>(),
            _ => throw new NotSupportedException("The provided message type is not supported."),
        };

        pushMessage.CreateFormatter()
            .FormatBody();

        _logger.LogInformation("Formatted push notification text:\n{content}", pushMessage.BodyContent);
        return Task.CompletedTask;
    }
}