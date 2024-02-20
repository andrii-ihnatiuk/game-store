using System.Diagnostics.CodeAnalysis;
using Azure.Messaging.ServiceBus;
using GameStore.Messaging.Constants;

namespace GameStore.MessageConsumer.Util;

[ExcludeFromCodeCoverage]
public static class ConsumerUtil
{
    public static string GetMessageTypeName(ServiceBusReceivedMessage message)
    {
        if (!message.ApplicationProperties.TryGetValue(MessageProperties.MessageType, out object property)
            || property is not string messageType)
        {
            throw new ArgumentException("MessageType property is not present or is not a string value.");
        }

        return messageType;
    }
}