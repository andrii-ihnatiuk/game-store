using GameStore.MessageConsumer.Interfaces;
using GameStore.MessageConsumer.Interfaces.MessageInitializers;
using GameStore.MessageConsumer.MessageInitializers.Order;

namespace GameStore.MessageConsumer.Messages.Order;

public class OrderStatusMessage : IEmailMessage
{
    public string OrderId { get; set; }

    public string Status { get; set; }

    public string RecipientName { get; set; }

    public string BodyContent { get; set; }

    public string RecipientEmail { get; set; }

    public string EmailSubject { get; set; }

    IEmailInitializer IEmailMessage.CreateInitializer()
    {
        return new OrderStatusEmailInitializer(this);
    }
}