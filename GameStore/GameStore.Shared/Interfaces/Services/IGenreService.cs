using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;

namespace GameStore.Shared.Interfaces.Services;

public interface IGenreService : IResolvableByEntityStorage
{
    Task<GenreFullDto> GetGenreByIdAsync(string id, string culture);

    Task<IList<GenreBriefDto>> GetAllGenresAsync(string culture);

    Task<IList<GenreBriefDto>> GetSubgenresByParentAsync(string parentId, string culture);

    Task<IList<GameBriefDto>> GetGamesByGenreIdAsync(string id, string culture);

    Task DeleteGenreAsync(string genreId);
}