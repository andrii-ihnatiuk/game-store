using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Interfaces;

namespace GameStore.Services.Extensions;

[ExcludeFromCodeCoverage]
public static class MapperExtensions
{
    public static TEntity MapTranslation<TEntity, TTranslation>(this IMapper mapper, TEntity entity, string culture)
        where TEntity : IMultiLingualEntity<TTranslation>
        where TTranslation : class, IEntityTranslation
    {
        var translation = entity.Translations.SingleOrDefault(t => t.LanguageCode == culture);
        if (translation is not null)
        {
            entity = mapper.Map(translation, entity);
        }

        return entity;
    }

    public static TDestination MapWithTranslation<TDestination, TTranslation>(
        this IMapper mapper,
        IMultiLingualEntity<TTranslation> entity,
        string culture)
            where TTranslation : class, IEntityTranslation
    {
        var translatedEntity = mapper.MapTranslation<IMultiLingualEntity<TTranslation>, TTranslation>(entity, culture);
        return mapper.Map<TDestination>(translatedEntity);
    }

    public static TDestination MapWithTranslation<TDestination, TTranslation>(
        this IMapper mapper,
        IEnumerable<IMultiLingualEntity<TTranslation>> entities,
        string culture)
            where TTranslation : class, IEntityTranslation
    {
        foreach (var entity in entities)
        {
            mapper.MapTranslation<IMultiLingualEntity<TTranslation>, TTranslation>(entity, culture);
        }

        return mapper.Map<TDestination>(entities);
    }

    public static TDestination MapWithoutLocalizedFields<TSource, TDestination>(
        this IMapper mapper,
        TSource source,
        TDestination destination)
    {
        return mapper.Map(source, destination, opts => opts.Items.Add("withoutLocalized", true));
    }
}