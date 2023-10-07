using GameStore.Shared.DTOs.Genre;

namespace GameStore.Services.Services;

public interface IGenreService
{
    Task<GenreViewFullDto> AddGenreAsync(GenreCreateDto dto);
}