using AutoMapper;
using GameStore.Data.Interfaces;
using GameStore.Services.Extensions;

namespace GameStore.Services;

public abstract class MultiLingualEntityServiceBase<TEntity, TTranslation>
    where TEntity : IMultiLingualEntity<TTranslation>
    where TTranslation : class, IEntityTranslation
{
    protected MultiLingualEntityServiceBase(IMapper mapper)
    {
        Mapper = mapper;
    }

    protected IMapper Mapper { get; }

    protected virtual TEntity UpdateMultiLingualEntity(TEntity entity, object updateDto, string culture)
    {
        if (culture == "en")
        {
            return Mapper.Map(updateDto, entity);
        }

        Mapper.MapWithoutLocalizedFields(updateDto, entity);
        TTranslation translation = entity.Translations.SingleOrDefault(t => t.LanguageCode == culture);
        if (translation is null)
        {
            translation = Mapper.Map<TTranslation>(updateDto);
            entity.Translations.Add(translation);
        }
        else
        {
            Mapper.Map(updateDto, translation);
        }

        return entity;
    }
}