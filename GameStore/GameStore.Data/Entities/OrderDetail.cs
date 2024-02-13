using GameStore.Data.Interfaces;

namespace GameStore.Data.Entities;

public class OrderDetail : ICreationTrackable
{
    public Guid Id { get; set; }

    public string ProductName { get; set; }

    public decimal Price { get; set; }

    public decimal FinalPrice { get; set; }

    public ushort Quantity { get; set; }

    public ushort Discount { get; set; }

    public DateTime CreationDate { get; set; }

    public Guid ProductId { get; set; }

    public Guid OrderId { get; set; }

    public Game Product { get; set; }

    public Order Order { get; set; }
}