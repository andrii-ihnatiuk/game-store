using GameStore.Data.Interfaces;

namespace GameStore.Data.Entities.Localization;

public class GameTranslation : IEntityTranslation<Game>
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? Type { get; set; }

    public string? Description { get; set; }

    public string LanguageCode { get; set; }

    public Guid CoreId { get; set; }

    public Game Core { get; set; }
}