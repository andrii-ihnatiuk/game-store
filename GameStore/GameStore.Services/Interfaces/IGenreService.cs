using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;

namespace GameStore.Services.Interfaces;

public interface IGenreService
{
    Task<GenreFullDto> GetGenreByIdAsync(Guid id);

    Task<IList<GenreBriefDto>> GetAllGenresAsync();

    Task<IList<GenreBriefDto>> GetSubgenresByParentAsync(Guid parentId);

    Task<IList<GameBriefDto>> GetGamesByGenreId(Guid id);

    Task<GenreBriefDto> AddGenreAsync(GenreCreateDto dto);

    Task UpdateGenreAsync(GenreUpdateDto dto);

    Task DeleteGenreAsync(Guid genreId);
}