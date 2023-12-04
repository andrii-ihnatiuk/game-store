using GameStore.Shared.Interfaces;

namespace GameStore.Shared.DTOs.Genre;

public class GenreBriefDto : ILegacyFilterable
{
    public string Id { get; set; }

    public string? LegacyId { get; set; }

    public string Name { get; set; }

    public string? ParentGenreId { get; set; }
}