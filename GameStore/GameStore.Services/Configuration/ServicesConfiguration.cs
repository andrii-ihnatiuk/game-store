using System.Diagnostics.CodeAnalysis;
using GameStore.Services.Authentication;
using GameStore.Services.Interfaces;
using GameStore.Services.Interfaces.Authentication;
using GameStore.Services.Interfaces.Payment;
using GameStore.Services.Payment;
using GameStore.Services.Payment.Strategies;
using GameStore.Shared.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Services.Configuration;

[ExcludeFromCodeCoverage]
public static class ServicesConfiguration
{
    public static void AddCoreServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IGameService, CoreGameService>();
        serviceCollection.AddScoped<ICoreGameService, CoreGameService>();

        serviceCollection.AddScoped<IGenreService, CoreGenreService>();
        serviceCollection.AddScoped<ICoreGenreService, CoreGenreService>();

        serviceCollection.AddScoped<IOrderService, CoreOrderService>();
        serviceCollection.AddScoped<ICoreOrderService, CoreOrderService>();

        serviceCollection.AddScoped<IPublisherService, CorePublisherService>();
        serviceCollection.AddScoped<ICorePublisherService, CorePublisherService>();

        serviceCollection.AddScoped<IPlatformService, PlatformService>();
        serviceCollection.AddScoped<IPaymentService, PaymentService>();
        serviceCollection.AddScoped<ICommentService, CommentService>();
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<IRoleService, RoleService>();

        serviceCollection.AddScoped<IJwtProvider, JwtProvider>();
        serviceCollection.AddScoped<ILoginService, InternalLoginService>();
        serviceCollection.AddScoped<ILoginService, ExternalLoginService>();

        serviceCollection.AddScoped<IPaymentStrategy, BankPaymentStrategy>();
        serviceCollection.AddScoped<IPaymentStrategy, TerminalPaymentStrategy>();
        serviceCollection.AddScoped<IPaymentStrategy, VisaPaymentStrategy>();
        serviceCollection.AddScoped<IPaymentStrategyResolver, PaymentStrategyResolver>();
    }
}