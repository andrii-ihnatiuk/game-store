using GameStore.MessageConsumer.Interfaces.MessageFormatters;

namespace GameStore.MessageConsumer.Interfaces;

public interface ISmsMessage : IMessage
{
    public string RecipientPhoneNumber { get; set; }

    ISmsFormatter CreateFormatter();
}