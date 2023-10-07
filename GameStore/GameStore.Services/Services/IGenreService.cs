using GameStore.Shared.DTOs.Genre;

namespace GameStore.Services.Services;

public interface IGenreService
{
    Task<GenreViewFullDto> GetGenreByIdAsync(long id);

    Task<GenreViewFullDto> AddGenreAsync(GenreCreateDto dto);

    Task UpdateGenreAsync(GenreUpdateDto dto);

    Task DeleteGenreAsync(long genreId);
}