using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using Northwind.Data;
using Northwind.Data.Interfaces;
using Northwind.Data.Repositories;
using Northwind.Services.Interfaces;

namespace Northwind.Services.Configuration;

[ExcludeFromCodeCoverage]
public static class ServicesConfiguration
{
    public static void AddNorthwindServices(this IServiceCollection serviceCollection)
    {
        var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
        ConventionRegistry.Register("IgnoreExtraElements", conventionPack, _ => true);

        serviceCollection.AddScoped<IMongoUnitOfWork, MongoUnitOfWork>();
        serviceCollection.AddScoped<IMongoContext, MongoContext>();
        serviceCollection.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        serviceCollection.AddScoped<IOrderService, OrderService>();
    }
}