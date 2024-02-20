using GameStore.MessageConsumer.Interfaces.MessageFormatters;
using GameStore.MessageConsumer.Messages.Order;

namespace GameStore.MessageConsumer.MessageFormatters.Order;

public class OrderStatusSmsFormatter : ISmsFormatter
{
    private readonly OrderStatusMessage _message;

    public OrderStatusSmsFormatter(OrderStatusMessage message)
    {
        _message = message;
    }

    public ISmsFormatter FormatBody()
    {
        _message.BodyContent = "This is an example text for SMS message.\n" +
                               "It is hardcoded for now and should be placed somewhere else.\n\n" +
                               $"Hello, {_message.RecipientName}!\n" +
                               $"The status of order {_message.OrderId} has changed to: {_message.Status}.";
        return this;
    }
}