namespace GameStore.Services.Models;

public class TerminalResponse
{
    public Guid AccountNumber { get; set; }

    public Guid InvoiceNumber { get; set; }

    public short PaymentMethod { get; set; }

    public Guid AccountId { get; set; }

    public decimal Amount { get; set; }
}