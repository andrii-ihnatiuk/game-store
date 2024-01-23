namespace GameStore.MessageConsumer.Interfaces.MessageInitializers;

public interface IEmailInitializer
{
    IEmailInitializer InitBody();

    IEmailInitializer InitSubject();
}