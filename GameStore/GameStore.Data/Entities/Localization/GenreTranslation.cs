using GameStore.Data.Interfaces;

namespace GameStore.Data.Entities.Localization;

public class GenreTranslation : IEntityTranslation<Genre>
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string LanguageCode { get; set; }

    public Guid CoreId { get; set; }

    public Genre Core { get; set; }
}