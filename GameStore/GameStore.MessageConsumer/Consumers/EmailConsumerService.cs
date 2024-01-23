using Azure.Messaging.ServiceBus;
using GameStore.MessageConsumer.Interfaces;
using GameStore.MessageConsumer.Messages.Order;
using GameStore.MessageConsumer.Options;
using GameStore.Messaging.Constants;
using GameStore.Messaging.DTOs.Order;
using Microsoft.Extensions.Options;

namespace GameStore.MessageConsumer.Consumers;

public class EmailConsumerService : AzureTopicConsumerBase
{
    private readonly ILogger<EmailConsumerService> _logger;
    private readonly ServiceBusOptions _serviceBusOptions;

    public EmailConsumerService(
        IOptions<ServiceBusOptions> serviceBusOptions,
        ILogger<EmailConsumerService> logger,
        IServiceScopeFactory scopeFactory)
        : base(logger, scopeFactory)
    {
        _logger = logger;
        _serviceBusOptions = serviceBusOptions.Value;
    }

    protected override string ConnectionString => _serviceBusOptions.ConnectionString;

    protected override string TopicName => _serviceBusOptions.TopicName;

    protected override string SubscriptionName => _serviceBusOptions.EmailSubscriptionName;

    protected override async Task ProcessMessageInternalAsync(ServiceBusReceivedMessage message, IServiceProvider provider)
    {
        _logger.LogInformation("Processing message with body:\n{body}", message.Body);
        var emailService = provider.GetRequiredService<IEmailService>();

        if (!message.ApplicationProperties.TryGetValue(MessageProperties.MessageType, out object property)
            || property is not string messageType)
        {
            throw new ArgumentException("MessageType property is not present or is not a string value.");
        }

        IEmailMessage emailMessage = messageType switch
        {
            nameof(OrderStatusMessageDto) => message.Body.ToObjectFromJson<OrderStatusMessage>(),
            _ => throw new NotSupportedException("The provided message type is not supported."),
        };

        emailMessage.CreateInitializer()
            .InitBody()
            .InitSubject();

        _logger.LogInformation("Created body content:\n{content}", emailMessage.BodyContent);
        await emailService.SendAsync(emailMessage);
    }
}