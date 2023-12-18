using GameStore.Shared.Interfaces;

namespace GameStore.Shared.DTOs.Publisher;

public class PublisherBriefDto : ILegacyFilterable
{
    public string Id { get; set; }

    public string? LegacyId { get; set; }

    public string CompanyName { get; set; }

    public string Description { get; set; }

    public string HomePage { get; set; }
}