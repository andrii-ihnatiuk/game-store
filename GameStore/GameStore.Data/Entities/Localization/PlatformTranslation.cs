using GameStore.Data.Interfaces;

namespace GameStore.Data.Entities.Localization;

public class PlatformTranslation : IEntityTranslation<Platform>
{
    public Guid Id { get; set; }

    public string Type { get; set; }

    public string LanguageCode { get; set; }

    public Guid CoreId { get; set; }

    public Platform Core { get; set; }
}