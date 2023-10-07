namespace GameStore.Shared.DTOs.Game;

public class GameUpdateDto
{
    public long GameId { get; set; }

    public string Alias { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }
}