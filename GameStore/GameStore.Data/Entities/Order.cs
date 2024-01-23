using GameStore.Data.Entities.Identity;
using GameStore.Shared.Constants;

namespace GameStore.Data.Entities;

public class Order
{
    public Guid Id { get; set; }

    public string CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? PaidDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Open;

    public decimal Sum { get; set; }

    public Guid? PaymentMethodId { get; set; }

    public PaymentMethod? PaymentMethod { get; set; }

    public IList<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public ApplicationUser Customer { get; set; }
}