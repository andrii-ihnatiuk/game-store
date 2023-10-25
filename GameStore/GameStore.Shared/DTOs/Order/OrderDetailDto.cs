namespace GameStore.Shared.DTOs.Order;

public class OrderDetailDto
{
    public Guid Id { get; set; }

    public decimal Price { get; set; }

    public short Quantity { get; set; }

    public float Discount { get; set; }

    public Guid ProductId { get; set; }
}