using GameStore.MessageConsumer.Interfaces;
using GameStore.MessageConsumer.Interfaces.MessageFormatters;
using GameStore.MessageConsumer.MessageFormatters.Order;

namespace GameStore.MessageConsumer.Messages.Order;

public class OrderStatusMessage : IEmailMessage, ISmsMessage, IPushMessage
{
    public string OrderId { get; set; }

    public string Status { get; set; }

    public string RecipientName { get; set; }

    public string BodyContent { get; set; }

    public string RecipientEmail { get; set; }

    public string EmailSubject { get; set; }

    public string RecipientPhoneNumber { get; set; }

    public string RecipientDeviceToken { get; set; }

    IEmailFormatter IEmailMessage.CreateFormatter()
    {
        return new OrderStatusEmailFormatter(this);
    }

    ISmsFormatter ISmsMessage.CreateFormatter()
    {
        return new OrderStatusSmsFormatter(this);
    }

    IPushFormatter IPushMessage.CreateFormatter()
    {
        return new OrderStatusPushFormatter(this);
    }
}