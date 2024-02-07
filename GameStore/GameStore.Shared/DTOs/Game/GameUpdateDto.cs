using GameStore.Shared.DTOs.Image;

namespace GameStore.Shared.DTOs.Game;

public class GameUpdateDto
{
    public string Culture { get; set; }

    public GameUpdateInnerDto Game { get; set; }

    public IList<string>? Genres { get; set; }

    public IList<Guid>? Platforms { get; set; }

    public string? Publisher { get; set; }

    public IList<ImageDto>? Images { get; set; }
}