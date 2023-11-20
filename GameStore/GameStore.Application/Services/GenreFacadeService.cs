using GameStore.Application.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Application.Services;

public class GenreFacadeService : IGenreFacadeService
{
    private readonly IServiceResolver _serviceResolver;

    public GenreFacadeService(IServiceResolver serviceResolver)
    {
        _serviceResolver = serviceResolver;
    }

    public async Task<IList<GenreBriefDto>> GetAllGenresAsync()
    {
        var services = _serviceResolver.ResolveAll<IGenreService>();
        var tasks = services.Select(s => s.GetAllGenresAsync()).ToList();
        await Task.WhenAll(tasks);

        var genres = tasks.SelectMany(t => t.Result).ToList();
        return genres;
    }

    public async Task<GenreFullDto> GetGenreByIdAsync(string id)
    {
        var genreService = _serviceResolver.ResolveForEntityId<IGenreService>(id);
        return await genreService.GetGenreByIdAsync(id);
    }

    public async Task<IList<GenreBriefDto>> GetSubgenresByParentAsync(string parentId)
    {
        var genreService = _serviceResolver.ResolveForEntityId<IGenreService>(parentId);
        return await genreService.GetSubgenresByParentAsync(parentId);
    }

    public async Task<IList<GameBriefDto>> GetGamesByGenreId(string id)
    {
        var genreService = _serviceResolver.ResolveForEntityId<IGenreService>(id);
        return await genreService.GetGamesByGenreId(id);
    }
}