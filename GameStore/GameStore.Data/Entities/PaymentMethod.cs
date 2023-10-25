namespace GameStore.Data.Entities;

public class PaymentMethod
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string ImageUrl { get; set; }
}