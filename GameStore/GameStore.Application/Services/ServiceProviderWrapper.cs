using System.Diagnostics.CodeAnalysis;
using GameStore.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Application.Services;

[ExcludeFromCodeCoverage]
public class ServiceProviderWrapper : IServiceProviderWrapper
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceProviderWrapper(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IEnumerable<T> GetServices<T>()
    {
        return _serviceProvider.GetServices<T>();
    }
}