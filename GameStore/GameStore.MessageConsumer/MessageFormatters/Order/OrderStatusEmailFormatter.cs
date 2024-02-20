using GameStore.MessageConsumer.Interfaces.MessageFormatters;
using GameStore.MessageConsumer.Messages.Order;

namespace GameStore.MessageConsumer.MessageFormatters.Order;

public class OrderStatusEmailFormatter : IEmailFormatter
{
    private readonly OrderStatusMessage _message;

    public OrderStatusEmailFormatter(OrderStatusMessage message)
    {
        _message = message;
    }

    public IEmailFormatter FormatBody()
    {
        _message.BodyContent = "<p>This is an example text for email message.</p>" +
                               "<p>It is hardcoded for now and <strong>should be placed somewhere else</strong>.</p>" +
                               $"<p style='margin-top:30px; font-style:bold'>Hello, {_message.RecipientName}!</p>" +
                               $"<p>The status of order {_message.OrderId} has changed to: {_message.Status}.</p>";
        return this;
    }

    public IEmailFormatter FormatSubject()
    {
        _message.EmailSubject = $"Status update for order №{_message.OrderId}";
        return this;
    }
}