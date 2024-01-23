using GameStore.MessageConsumer.Interfaces.MessageInitializers;
using GameStore.MessageConsumer.Messages.Order;

namespace GameStore.MessageConsumer.MessageInitializers.Order;

public class OrderStatusEmailInitializer : IEmailInitializer
{
    private readonly OrderStatusMessage _message;

    public OrderStatusEmailInitializer(OrderStatusMessage message)
    {
        _message = message;
    }

    public IEmailInitializer InitBody()
    {
        _message.BodyContent = "<p>This is an example text for email message.</p>" +
               "<p>It is hardcoded for now and <strong>should be placed somewhere else</strong>.</p>" +
               $"<p style='margin-top:30px; font-style:bold'>Hello, {_message.RecipientName}!</p>" +
               $"<p>The status of your order №{_message.OrderId} has changed to: {_message.Status}.</p>";
        return this;
    }

    public IEmailInitializer InitSubject()
    {
        _message.EmailSubject = $"Status update for order №{_message.OrderId}";
        return this;
    }
}