using GameStore.MessageConsumer.Interfaces;
using GameStore.MessageConsumer.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace GameStore.MessageConsumer.Services;

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;

    public EmailService(IOptions<EmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.Value;
    }

    public async Task SendAsync(IEmailMessage message)
    {
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_emailOptions.Host, _emailOptions.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_emailOptions.Username, _emailOptions.Password);

        var mail = new MimeMessage();
        mail.From.Add(new MailboxAddress(_emailOptions.SenderName, _emailOptions.SenderEmail));
        mail.To.Add(new MailboxAddress(string.Empty, message.RecipientEmail));
        mail.Subject = message.EmailSubject;
        var body = new BodyBuilder
        {
            HtmlBody = message.BodyContent,
        };

        mail.Body = body.ToMessageBody();

        await smtp.SendAsync(mail);
        await smtp.DisconnectAsync(true);
    }
}