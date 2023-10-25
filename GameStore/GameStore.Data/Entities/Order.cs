namespace GameStore.Data.Entities;

public class Order
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; } = new Guid("43efd8db-5b4b-4fcf-94d6-7916c7263f43");

    public DateTime OrderDate { get; set; }

    public DateTime? PaidDate { get; set; }

    public decimal Sum { get; set; }

    public Guid? PaymentMethodId { get; set; }

    public PaymentMethod? PaymentMethod { get; set; }

    public IList<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}