using GameStore.Shared.Constants.Filter;

namespace GameStore.Shared.Models;

public class OrdersFilter
{
    public OrdersFilter(DateTime lowerDate, DateTime upperDate)
    {
        LowerDate = lowerDate;
        UpperDate = upperDate;
    }

    public OrdersFilter()
    {
    }

    public IList<string> Stores { get; set; } = new List<string> { StoreOption.GameStore, StoreOption.Northwind };

    public DateTime LowerDate { get; set; } = DateTime.MinValue;

    public DateTime UpperDate { get; set; } = DateTime.MaxValue;

    public string? CustomerId { get; set; }

    public bool OnlyPaid { get; set; }
}