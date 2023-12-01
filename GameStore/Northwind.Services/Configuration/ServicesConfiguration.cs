using System.Diagnostics.CodeAnalysis;
using GameStore.Shared.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using Northwind.Data;
using Northwind.Data.Interfaces;
using Northwind.Data.Logger;
using Northwind.Data.Repositories;

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
        serviceCollection.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
        serviceCollection.AddScoped<ICategoryRepository, CategoryRepository>();
        serviceCollection.AddScoped<ISupplierRepository, SupplierRepository>();

        serviceCollection.AddScoped<IOrderService, MongoOrderService>();
        serviceCollection.AddScoped<IGameService, MongoProductService>();
        serviceCollection.AddScoped<IGenreService, MongoCategoryService>();
        serviceCollection.AddScoped<IPublisherService, MongoSupplierService>();

        serviceCollection.AddScoped<IEntityLogger, EntityLogger>();
    }
}