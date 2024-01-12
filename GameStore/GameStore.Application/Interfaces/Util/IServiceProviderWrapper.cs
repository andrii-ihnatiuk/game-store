namespace GameStore.Application.Interfaces.Util;

public interface IServiceProviderWrapper
{
    IEnumerable<T> GetServices<T>();
}