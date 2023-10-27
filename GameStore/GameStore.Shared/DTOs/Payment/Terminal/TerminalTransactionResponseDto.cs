namespace GameStore.Shared.DTOs.Payment.Terminal;

public class TerminalTransactionResponseDto
{
    public Guid AccountNumber { get; set; }

    public Guid InvoiceNumber { get; set; }

    public short PaymentMethod { get; set; }

    public Guid AccountId { get; set; }

    public decimal Amount { get; set; }
}