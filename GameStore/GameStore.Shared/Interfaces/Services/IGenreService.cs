using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;

namespace GameStore.Shared.Interfaces.Services;

public interface IGenreService : IResolvableByEntityStorage
{
    Task<GenreFullDto> GetGenreByIdAsync(string id);

    Task<IList<GenreBriefDto>> GetAllGenresAsync();

    Task<IList<GenreBriefDto>> GetSubgenresByParentAsync(string parentId);

    Task<IList<GameBriefDto>> GetGamesByGenreId(string id);

    Task DeleteGenreAsync(string genreId);
}