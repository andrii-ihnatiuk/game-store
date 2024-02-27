using GameStore.Data.Interfaces;

namespace GameStore.Data.Entities.Localization;

public class PublisherTranslation : IEntityTranslation<Publisher>
{
    public Guid Id { get; set; }

    public string CompanyName { get; set; }

    public string Description { get; set; }

    public string LanguageCode { get; set; }

    public Guid CoreId { get; set; }

    public Publisher Core { get; set; }
}