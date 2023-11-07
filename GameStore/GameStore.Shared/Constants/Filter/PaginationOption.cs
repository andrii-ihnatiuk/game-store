using System.Diagnostics.CodeAnalysis;

namespace GameStore.Shared.Constants.Filter;

[ExcludeFromCodeCoverage]
public static class PaginationOption
{
    public const string Ten = "10";
    public const string Twenty = "20";
    public const string Fifty = "50";
    public const string Hundred = "100";
    public const string All = "all";

    public static readonly string[] AllOptions = { Ten, Twenty, Fifty, Hundred, All };

    public static int ToNumber(string option)
    {
        return !AllOptions.Contains(option)
            ? throw new ArgumentOutOfRangeException(nameof(option))
            : option == All ? int.MaxValue : int.Parse(option);
    }
}