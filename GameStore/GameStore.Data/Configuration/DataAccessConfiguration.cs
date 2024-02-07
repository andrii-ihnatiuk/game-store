using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Entities.Identity;
using GameStore.Data.Interceptors;
using GameStore.Data.Interfaces;
using GameStore.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Data.Configuration;

[ExcludeFromCodeCoverage]
public static class DataAccessConfiguration
{
    public static void AddGameStoreDataAccess(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<MongoLoggingInterceptor>();

        serviceCollection.AddDbContext<GameStoreDbContext>((sp, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlDb"));
            options.AddInterceptors(sp.GetRequiredService<MongoLoggingInterceptor>());
        });

        serviceCollection.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<GameStoreDbContext>()
            .AddDefaultTokenProviders();

        serviceCollection.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddScoped<IGameRepository, GameRepository>();
        serviceCollection.AddScoped<IOrderRepository, OrderRepository>();
    }
}