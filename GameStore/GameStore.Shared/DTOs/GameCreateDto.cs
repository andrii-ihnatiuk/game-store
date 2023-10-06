namespace GameStore.Shared.DTOs;

public class GameCreateDto
{
    public long GameId { get; set; }

    public string? Alias { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }
}