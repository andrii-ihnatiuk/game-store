using System.Diagnostics.CodeAnalysis;
using Azure.Messaging.ServiceBus;
using GameStore.MessageConsumer.Interfaces;
using GameStore.MessageConsumer.Messages.Order;
using GameStore.MessageConsumer.Options;
using GameStore.MessageConsumer.Util;
using GameStore.Messaging.DTOs.Order;
using Microsoft.Extensions.Options;

namespace GameStore.MessageConsumer.Consumers;

[ExcludeFromCodeCoverage]
public class SmsConsumerService : AzureTopicConsumerBase
{
    private readonly ILogger<SmsConsumerService> _logger;
    private readonly ServiceBusOptions _serviceBusOptions;

    public SmsConsumerService(
        IOptions<ServiceBusOptions> serviceBusOptions,
        ILogger<SmsConsumerService> logger,
        IServiceScopeFactory scopeFactory)
        : base(logger, scopeFactory)
    {
        _logger = logger;
        _serviceBusOptions = serviceBusOptions.Value;
    }

    protected override string ConnectionString => _serviceBusOptions.ConnectionString;

    protected override string TopicName => _serviceBusOptions.TopicName;

    protected override string SubscriptionName => _serviceBusOptions.SmsSubscriptionName;

    protected override Task ProcessMessageInternalAsync(ServiceBusReceivedMessage message, IServiceProvider provider)
    {
        _logger.LogInformation("Processing SMS message with body:\n{body}", message.Body);

        ISmsMessage smsMessage = ConsumerUtil.GetMessageTypeName(message) switch
        {
            nameof(OrderStatusMessageDto) => message.Body.ToObjectFromJson<OrderStatusMessage>(),
            _ => throw new NotSupportedException("The provided message type is not supported."),
        };

        smsMessage.CreateFormatter()
            .FormatBody();

        _logger.LogInformation("Formatted SMS text:\n{content}", smsMessage.BodyContent);
        return Task.CompletedTask;
    }
}