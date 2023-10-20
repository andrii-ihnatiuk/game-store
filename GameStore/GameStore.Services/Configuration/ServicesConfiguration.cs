using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentValidation;
using GameStore.Data;
using GameStore.Data.Repositories;
using GameStore.Services.Services;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.Loggers;
using GameStore.Shared.Validators;
using GameStore.Shared.Validators.GameValidators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Services.Configuration;

[ExcludeFromCodeCoverage]
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

        serviceCollection.AddSingleton<ILogger, NLogLogger>();

        serviceCollection.AddScoped<IValidator<GameCreateDto>, GameCreateValidator>();
        serviceCollection.AddScoped(typeof(IValidatorWrapper<>), typeof(ValidatorWrapper<>));
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");
    }
}