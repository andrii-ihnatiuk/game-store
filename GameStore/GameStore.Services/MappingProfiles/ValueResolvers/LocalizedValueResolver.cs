using System.Diagnostics.CodeAnalysis;
using AutoMapper;

namespace GameStore.Services.MappingProfiles.ValueResolvers;

[ExcludeFromCodeCoverage]
public class LocalizedValueResolver<TValue> : IMemberValueResolver<object, object, TValue, TValue>
{
    public TValue Resolve(object source, object destination, TValue sourceMember, TValue destMember, ResolutionContext context)
    {
        return context.TryGetItems(out var items)
               && items.TryGetValue("withoutLocalized", out object withoutLocalized)
               && withoutLocalized is true
            ? destMember
            : sourceMember;
    }
}