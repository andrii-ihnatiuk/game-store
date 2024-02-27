using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;

namespace GameStore.Application.Interfaces;

public interface IGenreFacadeService
{
    Task<IList<GenreBriefDto>> GetAllGenresAsync(string culture);

    Task<GenreFullDto> GetGenreByIdAsync(string id, string culture);

    Task<IList<GenreBriefDto>> GetSubgenresByParentAsync(string parentId, string culture);

    Task<IList<GameBriefDto>> GetGamesByGenreIdAsync(string id, string culture);

    Task<GenreBriefDto> AddGenreAsync(GenreCreateDto dto);

    Task UpdateGenreAsync(GenreUpdateDto dto);

    Task DeleteGenreAsync(string genreId);
}