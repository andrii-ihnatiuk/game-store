using System.Diagnostics.CodeAnalysis;
using GameStore.Shared.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Data.Interfaces;
using Northwind.Data.Logger;

namespace Northwind.Services.Configuration;

[ExcludeFromCodeCoverage]
public static class ServicesConfiguration
{
    public static void AddNorthwindServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IOrderService, MongoOrderService>();
        serviceCollection.AddScoped<IGameService, MongoProductService>();
        serviceCollection.AddScoped<IGenreService, MongoCategoryService>();
        serviceCollection.AddScoped<IPublisherService, MongoSupplierService>();

        serviceCollection.AddScoped<IEntityLogger, EntityLogger>();
    }
}