namespace GameStore.Data.Interfaces;

public interface IEntityTranslation
{
    string LanguageCode { get; set; }
}

public interface IEntityTranslation<TEntity> : IEntityTranslation
{
    Guid CoreId { get; set; }

    TEntity Core { get; set; }
}