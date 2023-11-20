using GameStore.Shared.DTOs.Game;

namespace GameStore.Shared.DTOs.Genre;

public class GenreFullDto
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string? ParentGenreId { get; set; }

    public IList<GenreBriefDto> SubGenres { get; set; } = new List<GenreBriefDto>();

    public IList<GameBriefDto> Games { get; set; } = new List<GameBriefDto>();
}