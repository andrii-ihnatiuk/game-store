namespace GameStore.Shared.DTOs.Order;

public class OrderBriefDto
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public DateTime OrderDate { get; set; }
}