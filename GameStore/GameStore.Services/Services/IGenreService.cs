using GameStore.Shared.DTOs.Genre;

namespace GameStore.Services.Services;

public interface IGenreService
{
    Task<GenreFullDto> GetGenreByIdAsync(Guid id);

    Task<IList<GenreBriefDto>> GetAllGenresAsync();

    Task<GenreFullDto> AddGenreAsync(GenreCreateDto dto);

    Task UpdateGenreAsync(GenreUpdateDto dto);

    Task DeleteGenreAsync(Guid genreId);
}