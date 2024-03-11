namespace GameStore.MessageConsumer.Interfaces.MessageFormatters;

public interface IEmailFormatter
{
    IEmailFormatter FormatBody();

    IEmailFormatter FormatSubject();
}