namespace GameStore.Shared.DTOs.Game;

public class GameBriefDto
{
    public Guid Id { get; set; }

    public string Key { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}