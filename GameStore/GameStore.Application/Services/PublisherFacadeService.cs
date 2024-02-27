using GameStore.Application.Interfaces;
using GameStore.Application.Interfaces.Migration;
using GameStore.Application.Interfaces.Util;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Extensions;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Application.Services;

public class PublisherFacadeService : IPublisherFacadeService
{
    private readonly IEntityServiceResolver _serviceResolver;
    private readonly IPublisherMigrationService _migrationService;

    public PublisherFacadeService(
        IEntityServiceResolver serviceResolver,
        IPublisherMigrationService migrationService)
    {
        _serviceResolver = serviceResolver;
        _migrationService = migrationService;
    }

    public Task<PublisherFullDto> GetPublisherByIdAsync(string id, string culture)
    {
        var service = _serviceResolver.ResolveForEntityId<IPublisherService>(id);
        return service.GetPublisherByIdAsync(id, culture);
    }

    public async Task<IList<PublisherBriefDto>> GetAllPublishersAsync(string culture)
    {
        var services = _serviceResolver.ResolveAll<IPublisherService>();
        var tasks = services.Select(s => s.GetAllPublishersAsync(culture)).ToList();
        await Task.WhenAll(tasks);

        var publishers = tasks.SelectMany(t => t.Result).ToList();
        return publishers.FilterLegacyEntities();
    }

    public Task<IList<GameBriefDto>> GetGamesByPublisherIdAsync(string id, string culture)
    {
        var service = _serviceResolver.ResolveForEntityId<IPublisherService>(id);
        return service.GetGamesByPublisherIdAsync(id, culture);
    }

    public async Task<PublisherBriefDto> AddPublisherAsync(PublisherCreateDto dto)
    {
        var service = _serviceResolver.ResolveAll<ICorePublisherService>().Single();
        return await service.AddPublisherAsync(dto);
    }

    public async Task UpdatePublisherAsync(PublisherUpdateDto dto)
    {
        await _migrationService.MigrateOnUpdateAsync(dto);
        var service = _serviceResolver.ResolveAll<ICorePublisherService>().Single();
        await service.UpdatePublisherAsync(dto);
    }

    public Task DeletePublisherAsync(string id)
    {
        var service = _serviceResolver.ResolveForEntityId<IPublisherService>(id);
        return service.DeletePublisherAsync(id);
    }
}