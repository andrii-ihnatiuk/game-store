using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using Northwind.Data.Interfaces;
using Northwind.Data.Repositories;

namespace Northwind.Data.Configuration;

[ExcludeFromCodeCoverage]
public static class DataAccessConfiguration
{
    public static void AddNorthwindDataAccess(this IServiceCollection serviceCollection)
    {
        var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
        ConventionRegistry.Register("IgnoreExtraElements", conventionPack, _ => true);

        serviceCollection.AddScoped<IMongoUnitOfWork, MongoUnitOfWork>();
        serviceCollection.AddScoped<IMongoContext, MongoContext>();
        serviceCollection.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        serviceCollection.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
        serviceCollection.AddScoped<ICategoryRepository, CategoryRepository>();
        serviceCollection.AddScoped<ISupplierRepository, SupplierRepository>();
        serviceCollection.AddScoped<IProductRepository, ProductRepository>();
        serviceCollection.AddScoped<IOrderRepository, OrderRepository>();
    }
}