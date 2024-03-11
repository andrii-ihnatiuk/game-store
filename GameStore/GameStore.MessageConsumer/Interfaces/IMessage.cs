namespace GameStore.MessageConsumer.Interfaces;

public interface IMessage
{
    string RecipientName { get; set; }

    string BodyContent { get; set; }
}