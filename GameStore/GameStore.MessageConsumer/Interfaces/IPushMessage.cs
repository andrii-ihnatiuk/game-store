using GameStore.MessageConsumer.Interfaces.MessageFormatters;

namespace GameStore.MessageConsumer.Interfaces;

public interface IPushMessage : IMessage
{
    public string RecipientDeviceToken { get; set; }

    IPushFormatter CreateFormatter();
}