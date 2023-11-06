namespace GameStore.Shared.Constants.Filter;

public static class PublishDateOption
{
    public const string LastWeek = "last week";
    public const string LastMonth = "last month";
    public const string LastYear = "last year";
    public const string LastTwoYears = "2 years";
    public const string LastThreeYears = "3 years";

    public static readonly string[] AllOptions = { LastWeek, LastMonth, LastYear, LastTwoYears, LastThreeYears };
}