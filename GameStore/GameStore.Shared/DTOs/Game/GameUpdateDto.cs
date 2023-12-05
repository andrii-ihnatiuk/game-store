namespace GameStore.Shared.DTOs.Game;

public class GameUpdateDto
{
    public GameUpdateInnerDto Game { get; set; }

    public IList<string>? Genres { get; set; }

    public IList<Guid>? Platforms { get; set; }

    public string? Publisher { get; set; }
}