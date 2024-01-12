namespace GameStore.Shared.DTOs.Order;

public class OrderBriefDto
{
    public string Id { get; set; }

    public string CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? ShippedDate { get; set; }
}