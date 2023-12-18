using System.Diagnostics.CodeAnalysis;
using GameStore.Shared.Interfaces;

namespace GameStore.Shared.Extensions;

[ExcludeFromCodeCoverage]
public static class ListExtensions
{
    public static IList<T> FilterLegacyEntities<T>(this IList<T> list)
        where T : ILegacyFilterable
    {
        return list.Where(i => list.All(e => e.LegacyId != i.Id)).ToList();
    }
}