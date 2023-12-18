using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;

namespace GameStore.Application.Interfaces;

public interface IGenreFacadeService
{
    Task<IList<GenreBriefDto>> GetAllGenresAsync();

    Task<GenreFullDto> GetGenreByIdAsync(string id);

    Task<IList<GenreBriefDto>> GetSubgenresByParentAsync(string parentId);

    Task<IList<GameBriefDto>> GetGamesByGenreIdAsync(string id);

    Task<GenreBriefDto> AddGenreAsync(GenreCreateDto dto);

    Task UpdateGenreAsync(GenreUpdateDto dto);

    Task DeleteGenreAsync(string genreId);
}