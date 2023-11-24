using GameStore.Shared.DTOs.Game;

namespace GameStore.Shared.DTOs.Publisher;

public class PublisherFullDto
{
    public string Id { get; set; }

    public string CompanyName { get; set; }

    public string Description { get; set; }

    public string HomePage { get; set; }

    public IList<GameBriefDto> Games { get; set; } = new List<GameBriefDto>();
}