using System.Diagnostics.CodeAnalysis;

namespace Northwind.Services.Util;

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
        return alias.EndsWith(AliasSuffix, StringComparison.InvariantCulture) ? alias[..^AliasSuffix.Length] : alias;
    }
}