namespace GameStore.MessageConsumer.Interfaces;

public interface IEmailService
{
    Task SendAsync(IEmailMessage message);
}