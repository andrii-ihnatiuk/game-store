namespace GameStore.Shared.DTOs.Game;

public class GameCreateDto
{
    public GameInnerDto Game { get; set; }

    public string? Publisher { get; set; }

    public IList<Guid>? Genres { get; set; }

    public IList<Guid>? Platforms { get; set; }
}