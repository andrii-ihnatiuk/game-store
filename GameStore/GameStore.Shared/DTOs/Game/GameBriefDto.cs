namespace GameStore.Shared.DTOs.Game;

public class GameBriefDto
{
    public long GameId { get; set; }

    public string Alias { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}