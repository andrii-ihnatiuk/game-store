using System.Diagnostics.CodeAnalysis;

namespace GameStore.Shared.Util;

[ExcludeFromCodeCoverage]
public static class EntityAliasUtil
{
    public const string AliasSuffix = "~northwind";

    public static string AddSuffix(string alias)
    {
        return string.Concat(alias, AliasSuffix);
    }

    public static string RemoveSuffix(string alias)
    {
        return ContainsSuffix(alias) ? alias[..^AliasSuffix.Length] : alias;
    }

    public static bool ContainsSuffix(string alias)
    {
        return alias.EndsWith(AliasSuffix, StringComparison.InvariantCulture);
    }
}