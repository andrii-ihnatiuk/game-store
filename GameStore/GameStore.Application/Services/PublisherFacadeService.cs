using GameStore.Application.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Application.Services;

public class PublisherFacadeService : IPublisherFacadeService
{
    private readonly IServiceResolver _serviceResolver;

    public PublisherFacadeService(IServiceResolver serviceResolver)
    {
        _serviceResolver = serviceResolver;
    }

    public async Task<PublisherFullDto> GetPublisherByNameAsync(string companyName)
    {
        var service = _serviceResolver.ResolveForEntityAlias<IPublisherService>(companyName);
        return await service.GetPublisherByNameAsync(companyName);
    }

    public async Task<IList<PublisherBriefDto>> GetAllPublishersAsync()
    {
        var services = _serviceResolver.ResolveAll<IPublisherService>();
        var tasks = services.Select(s => s.GetAllPublishersAsync()).ToList();
        await Task.WhenAll(tasks);

        var publishers = tasks.SelectMany(t => t.Result).ToList();
        return publishers;
    }

    public async Task<IList<GameBriefDto>> GetGamesByPublisherNameAsync(string companyName)
    {
        var service = _serviceResolver.ResolveForEntityAlias<IPublisherService>(companyName);
        return await service.GetGamesByPublisherNameAsync(companyName);
    }

    public Task UpdatePublisherAsync(PublisherUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task DeletePublisherAsync(string id)
    {
        throw new NotImplementedException();
    }
}