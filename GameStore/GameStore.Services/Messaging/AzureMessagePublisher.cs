using System.Text;
using Azure.Messaging.ServiceBus;
using GameStore.Services.Interfaces.Messaging;
using GameStore.Services.Models;
using Newtonsoft.Json;

namespace GameStore.Services.Messaging;

public class AzureMessagePublisher : IMessagePublisher
{
    private readonly ServiceBusClient _serviceBus;

    public AzureMessagePublisher(ServiceBusClient serviceBus)
    {
        _serviceBus = serviceBus;
    }

    public async Task PublishAsync<T>(TopicMessage<T> message)
        where T : class
    {
        await using var sender = _serviceBus.CreateSender(message.TopicName);
        string serialized = JsonConvert.SerializeObject(message.Value);
        var busMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(serialized));

        foreach (var property in message.Properties)
        {
            busMessage.ApplicationProperties[property.Key] = property.Value;
        }

        await sender.SendMessageAsync(busMessage);
    }
}