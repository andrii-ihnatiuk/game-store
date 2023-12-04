using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Services.Interfaces;

public interface ICoreGenreService : IGenreService
{
    Task<GenreBriefDto> AddGenreAsync(GenreCreateDto dto);

    Task UpdateGenreAsync(GenreUpdateDto dto);
}