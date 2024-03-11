using Azure.Messaging.ServiceBus;

namespace GameStore.MessageConsumer.Consumers;

public abstract class AzureTopicConsumerBase : BackgroundService
{
    private const int DefaultDelay = 3000;
    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    protected AzureTopicConsumerBase(ILogger logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected abstract string ConnectionString { get; }

    protected abstract string TopicName { get; }

    protected abstract string SubscriptionName { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var client = new ServiceBusClient(ConnectionString);
        await using var processor = client.CreateProcessor(TopicName, SubscriptionName);

        processor.ProcessMessageAsync += ProcessMessageAsync;
        processor.ProcessErrorAsync += ProcessErrorAsync;

        _logger.LogInformation("Starting processing incoming messages...");
        await processor.StartProcessingAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(DefaultDelay, stoppingToken);
        }

        _logger.LogInformation("Stopping processing messages...");
        await processor.StopProcessingAsync(stoppingToken);
    }

    protected abstract Task ProcessMessageInternalAsync(ServiceBusReceivedMessage message, IServiceProvider provider);

    private async Task ProcessMessageAsync(ProcessMessageEventArgs eventArgs)
    {
        _logger.LogInformation("Received new message");

        using var scope = _scopeFactory.CreateScope();
        await ProcessMessageInternalAsync(eventArgs.Message, scope.ServiceProvider);

        await eventArgs.CompleteMessageAsync(eventArgs.Message);
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs eventArgs)
    {
        _logger.LogError(eventArgs.Exception, eventArgs.Exception.ToString());
        return Task.CompletedTask;
    }
}