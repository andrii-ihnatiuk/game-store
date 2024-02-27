namespace GameStore.Data.Interfaces;

public interface IMultiLingualEntity<TTranslation>
    where TTranslation : class, IEntityTranslation
{
    IList<TTranslation> Translations { get; set; }
}