namespace GameStore.Application.Interfaces;

public interface IServiceProviderWrapper
{
    IEnumerable<T> GetServices<T>();
}