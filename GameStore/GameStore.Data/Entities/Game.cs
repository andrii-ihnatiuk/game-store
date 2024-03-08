using GameStore.Data.Entities.Localization;
using GameStore.Data.Interfaces;

namespace GameStore.Data.Entities;

public class Game : ICreationTrackable, IMigrationTrackable, IMultiLingualEntity<GameTranslation>
{
    public Guid Id { get; set; }

    public string? LegacyId { get; set; }

    public string Alias { get; set; }

    public string Name { get; set; }

    public string? Type { get; set; }

    public string? FileSize { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public short UnitInStock { get; set; }

    public bool Discontinued { get; set; }

    public bool Deleted { get; set; }

    public ushort Discount { get; set; }

    public ulong PageViews { get; set; }

    public DateTime? PublishDate { get; set; }

    public DateTime CreationDate { get; set; }

    public string? PreviewImgUrl { get; set; }

    public Guid? PublisherId { get; set; }

    public Publisher? Publisher { get; set; }

    public IList<GameGenre> GameGenres { get; set; } = new List<GameGenre>();

    public IList<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();

    public IList<Comment> Comments { get; set; } = new List<Comment>();

    public IList<GameTranslation> Translations { get; set; } = new List<GameTranslation>();

    public IList<AppImage> Images { get; set; } = new List<AppImage>();
}