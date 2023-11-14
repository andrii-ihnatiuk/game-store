namespace GameStore.Shared.DTOs.Order;

public class OrderDetailDto
{
    public decimal Price { get; set; }

    public short Quantity { get; set; }

    public float Discount { get; set; }

    public string ProductId { get; set; }
}