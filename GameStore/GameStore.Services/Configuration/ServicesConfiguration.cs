using GameStore.Data;
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

public static class ServicesConfiguration
{
    public static void AddCoreServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<GameStoreDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        serviceCollection.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddScoped<IGameRepository, GameRepository>();

        serviceCollection.AddScoped<IGameService, CoreGameService>();
        serviceCollection.AddScoped<ICoreGameService, CoreGameService>();
        serviceCollection.AddScoped<IGenreService, GenreService>();
        serviceCollection.AddScoped<IPlatformService, PlatformService>();
        serviceCollection.AddScoped<IPublisherService, PublisherService>();
        serviceCollection.AddScoped<IOrderService, CoreOrderService>();
        serviceCollection.AddScoped<ICoreOrderService, CoreOrderService>();
        serviceCollection.AddScoped<IPaymentService, PaymentService>();
        serviceCollection.AddScoped<ICommentService, CommentService>();

        serviceCollection.AddScoped<IPaymentStrategy, BankPaymentStrategy>();
        serviceCollection.AddScoped<IPaymentStrategy, TerminalPaymentStrategy>();
        serviceCollection.AddScoped<IPaymentStrategy, VisaPaymentStrategy>();
        serviceCollection.AddScoped<IPaymentStrategyResolver, PaymentStrategyResolver>();
    }
}