using System.Diagnostics.CodeAnalysis;
using GameStore.Data;
using GameStore.Data.Interceptors;
using GameStore.Data.Interfaces;
using GameStore.Data.Repositories;
using GameStore.Services.Interfaces;
using GameStore.Services.Interfaces.Payment;
using GameStore.Services.Payment;
using GameStore.Services.Payment.Strategies;
using GameStore.Shared.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Services.Configuration;

[ExcludeFromCodeCoverage]
public static class ServicesConfiguration
{
    public static void AddCoreServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<MongoLoggingInterceptor>();
        serviceCollection.AddDbContext<GameStoreDbContext>((sp, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.AddInterceptors(sp.GetRequiredService<MongoLoggingInterceptor>());
        });
        serviceCollection.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddScoped<IGameRepository, GameRepository>();

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

        serviceCollection.AddScoped<IPaymentStrategy, BankPaymentStrategy>();
        serviceCollection.AddScoped<IPaymentStrategy, TerminalPaymentStrategy>();
        serviceCollection.AddScoped<IPaymentStrategy, VisaPaymentStrategy>();
        serviceCollection.AddScoped<IPaymentStrategyResolver, PaymentStrategyResolver>();
    }
}