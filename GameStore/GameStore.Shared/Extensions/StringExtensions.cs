using System.Diagnostics.CodeAnalysis;

namespace GameStore.Shared.Extensions;

[ExcludeFromCodeCoverage]
public static class StringExtensions
{
    public static bool IsGuidFormat(this string? str)
    {
        return Guid.TryParse(str, out _);
    }

    public static bool IsNotGuidFormat(this string? str)
    {
        return !IsGuidFormat(str);
    }

    public static Guid? ToNullableGuid(this string? str)
    {
        return string.IsNullOrEmpty(str) ? null : new Guid(str);
    }
}