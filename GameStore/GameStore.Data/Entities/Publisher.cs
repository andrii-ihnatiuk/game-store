using GameStore.Data.Entities.Localization;
using GameStore.Data.Interfaces;

namespace GameStore.Data.Entities;

public class Publisher : IMigrationTrackable, IMultiLingualEntity<PublisherTranslation>
{
    public Guid Id { get; set; }

    public string? LegacyId { get; set; }

    public string CompanyName { get; set; }

    public string Description { get; set; }

    public string HomePage { get; set; }

    public string? AccountId { get; set; }

    public IList<Game> Games { get; set; } = new List<Game>();

    public IList<PublisherTranslation> Translations { get; set; } = new List<PublisherTranslation>();
}