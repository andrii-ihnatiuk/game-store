using GameStore.Data;
using GameStore.Data.Repositories;
using GameStore.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Services.Configuration;

public static class ServicesConfiguration
{
    public static void AddApplicationServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<GameStoreDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        serviceCollection.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        serviceCollection.AddScoped<IGameService, GameService>();
        serviceCollection.AddScoped<IGenreService, GenreService>();
        serviceCollection.AddScoped<IPlatformService, PlatformService>();
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}