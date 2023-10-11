namespace GameStore.Shared.DTOs.Game;

public class GameCreateDto
{
    public string? Alias { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public long? GenreId { get; set; }

    public long? PlatformId { get; set; }
}