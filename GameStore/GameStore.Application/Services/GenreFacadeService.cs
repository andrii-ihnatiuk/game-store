using GameStore.Application.Interfaces;
using GameStore.Application.Interfaces.Migration;
using GameStore.Application.Interfaces.Util;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.Extensions;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Application.Services;

public class GenreFacadeService : IGenreFacadeService
{
    private readonly IEntityServiceResolver _serviceResolver;
    private readonly IGenreMigrationService _migrationService;

    public GenreFacadeService(
        IEntityServiceResolver serviceResolver,
        IGenreMigrationService migrationService)
    {
        _serviceResolver = serviceResolver;
        _migrationService = migrationService;
    }

    public async Task<IList<GenreBriefDto>> GetAllGenresAsync(string culture)
    {
        var services = _serviceResolver.ResolveAll<IGenreService>();
        var tasks = services.Select(s => s.GetAllGenresAsync(culture)).ToList();
        await Task.WhenAll(tasks);

        var genres = tasks.SelectMany(t => t.Result).ToList();
        return genres.FilterLegacyEntities();
    }

    public Task<GenreFullDto> GetGenreByIdAsync(string id, string culture)
    {
        var genreService = _serviceResolver.ResolveForEntityId<IGenreService>(id);
        return genreService.GetGenreByIdAsync(id, culture);
    }

    public Task<IList<GenreBriefDto>> GetSubgenresByParentAsync(string parentId, string culture)
    {
        var genreService = _serviceResolver.ResolveForEntityId<IGenreService>(parentId);
        return genreService.GetSubgenresByParentAsync(parentId, culture);
    }

    public Task<IList<GameBriefDto>> GetGamesByGenreIdAsync(string id, string culture)
    {
        var genreService = _serviceResolver.ResolveForEntityId<IGenreService>(id);
        return genreService.GetGamesByGenreIdAsync(id, culture);
    }

    public async Task<GenreBriefDto> AddGenreAsync(GenreCreateDto dto)
    {
        await _migrationService.MigrateOnCreateAsync(dto);
        var service = _serviceResolver.ResolveAll<ICoreGenreService>().Single();
        return await service.AddGenreAsync(dto);
    }

    public async Task UpdateGenreAsync(GenreUpdateDto dto)
    {
        await _migrationService.MigrateOnUpdateAsync(dto);
        var service = _serviceResolver.ResolveAll<ICoreGenreService>().Single();
        await service.UpdateGenreAsync(dto);
    }

    public Task DeleteGenreAsync(string genreId)
    {
        var service = _serviceResolver.ResolveForEntityId<IGenreService>(genreId);
        return service.DeleteGenreAsync(genreId);
    }
}