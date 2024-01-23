using GameStore.MessageConsumer.Interfaces.MessageInitializers;

namespace GameStore.MessageConsumer.Interfaces;

public interface IEmailMessage : IMessage
{
    public string RecipientEmail { get; set; }

    public string EmailSubject { get; set; }

    IEmailInitializer CreateInitializer();
}