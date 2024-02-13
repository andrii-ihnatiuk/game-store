namespace GameStore.Shared.DTOs.Order;

public class CartDetailsDto
{
    public decimal Subtotal { get; set; }

    public decimal Total { get; set; }

    public decimal Taxes { get; set; }

    public IList<OrderDetailDto> Details { get; set; }
}