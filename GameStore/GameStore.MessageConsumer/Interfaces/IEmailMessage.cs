using GameStore.MessageConsumer.Interfaces.MessageFormatters;

namespace GameStore.MessageConsumer.Interfaces;

public interface IEmailMessage : IMessage
{
    public string RecipientEmail { get; set; }

    public string EmailSubject { get; set; }

    IEmailFormatter CreateFormatter();
}