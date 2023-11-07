namespace GameStore.Shared.Constants.Filter;

public static class SortingOption
{
    public const string Newest = "Newest";
    public const string MostPopular = "Most popular (most viewed)";
    public const string MostCommented = "Most commented";
    public const string PriceAsc = "Price ASC";
    public const string PriceDesc = "Price DESC";

    public static readonly string[] AllOptions = { Newest, MostPopular, MostCommented, PriceAsc, PriceDesc };
}