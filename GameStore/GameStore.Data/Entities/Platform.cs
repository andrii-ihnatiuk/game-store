using GameStore.Data.Entities.Localization;
using GameStore.Data.Interfaces;

namespace GameStore.Data.Entities;

public class Platform : IMultiLingualEntity<PlatformTranslation>
{
    public Guid Id { get; set; }

    public string Type { get; set; }

    public IList<GamePlatform> PlatformGames { get; set; } = new List<GamePlatform>();

    public IList<PlatformTranslation> Translations { get; set; } = new List<PlatformTranslation>();
}