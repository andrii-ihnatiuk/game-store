using GameStore.Shared.DTOs.Game;

namespace GameStore.Shared.DTOs.Platform;

public class PlatformFullDto
{
    public Guid Id { get; set; }

    public string Type { get; set; }

    public IList<GameBriefDto> Games { get; set; } = new List<GameBriefDto>();
}