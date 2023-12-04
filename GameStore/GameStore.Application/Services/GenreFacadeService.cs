using GameStore.Application.Interfaces;
using GameStore.Application.Interfaces.Migration;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.Extensions;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Application.Services;

public class GenreFacadeService : IGenreFacadeService
{
    private readonly IServiceResolver _serviceResolver;
    private readonly IEntityMigrationService<GenreUpdateDto, GenreCreateDto> _migrationService;

    public GenreFacadeService(
        IServiceResolver serviceResolver,
        IEntityMigrationService<GenreUpdateDto, GenreCreateDto> migrationService)
    {
        _serviceResolver = serviceResolver;
        _migrationService = migrationService;
    }

    public async Task<IList<GenreBriefDto>> GetAllGenresAsync()
    {
        var services = _serviceResolver.ResolveAll<IGenreService>();
        var tasks = services.Select(s => s.GetAllGenresAsync()).ToList();
        await Task.WhenAll(tasks);

        var genres = tasks.SelectMany(t => t.Result).ToList();
        return genres.FilterLegacyEntities();
    }

    public Task<GenreFullDto> GetGenreByIdAsync(string id)
    {
        var genreService = _serviceResolver.ResolveForEntityId<IGenreService>(id);
        return genreService.GetGenreByIdAsync(id);
    }

    public Task<IList<GenreBriefDto>> GetSubgenresByParentAsync(string parentId)
    {
        var genreService = _serviceResolver.ResolveForEntityId<IGenreService>(parentId);
        return genreService.GetSubgenresByParentAsync(parentId);
    }

    public Task<IList<GameBriefDto>> GetGamesByGenreIdAsync(string id)
    {
        var genreService = _serviceResolver.ResolveForEntityId<IGenreService>(id);
        return genreService.GetGamesByGenreId(id);
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