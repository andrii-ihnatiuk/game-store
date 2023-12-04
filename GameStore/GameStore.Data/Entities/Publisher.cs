using GameStore.Data.Interfaces;

namespace GameStore.Data.Entities;

public class Publisher : IMigrationTrackable
{
    public Guid Id { get; set; }

    public string? LegacyId { get; set; }

    public string CompanyName { get; set; }

    public string Description { get; set; }

    public string HomePage { get; set; }

    public IList<Game> Games { get; set; } = new List<Game>();
}