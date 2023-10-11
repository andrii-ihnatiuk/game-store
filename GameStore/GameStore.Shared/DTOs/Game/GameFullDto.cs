namespace GameStore.Shared.DTOs.Game;

public class GameFullDto
{
    public long GameId { get; set; }

    public string Alias { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}