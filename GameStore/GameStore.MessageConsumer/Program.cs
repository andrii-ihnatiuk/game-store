using GameStore.MessageConsumer.Consumers;
using GameStore.MessageConsumer.Interfaces;
using GameStore.MessageConsumer.Options;
using GameStore.MessageConsumer.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;
        services.Configure<ServiceBusOptions>(configuration.GetSection("Azure:ServiceBus"));
        services.Configure<EmailOptions>(configuration.GetSection("EmailSettings"));

        services.AddScoped<IEmailService, EmailService>();

        services.AddHostedService<EmailConsumerService>();
        services.AddHostedService<SmsConsumerService>();
        services.AddHostedService<PushConsumerService>();
    })
    .Build();

await host.RunAsync();