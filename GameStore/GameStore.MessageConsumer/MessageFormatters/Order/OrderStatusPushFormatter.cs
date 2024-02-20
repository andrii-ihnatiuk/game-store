using GameStore.MessageConsumer.Interfaces.MessageFormatters;
using GameStore.MessageConsumer.Messages.Order;

namespace GameStore.MessageConsumer.MessageFormatters.Order;

public class OrderStatusPushFormatter : IPushFormatter
{
    private readonly OrderStatusMessage _message;

    public OrderStatusPushFormatter(OrderStatusMessage message)
    {
        _message = message;
    }

    public IPushFormatter FormatBody()
    {
        _message.BodyContent = "This is an example text for push notification.\n" +
                               "It is hardcoded for now and should be placed somewhere else.\n\n" +
                               $"Hello, {_message.RecipientName}!\n" +
                               $"The status of order {_message.OrderId} has changed to: {_message.Status}.";
        return this;
    }
}