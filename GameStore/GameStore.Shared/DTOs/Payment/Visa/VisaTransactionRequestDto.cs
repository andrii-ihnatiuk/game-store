namespace GameStore.Shared.DTOs.Payment.Visa;

public class VisaTransactionRequestDto
{
    public decimal TransactionAmount { get; set; }

    public string CardHolderName { get; set; }

    public string CardNumber { get; set; }

    public ushort ExpirationMonth { get; set; }

    public uint Cvv { get; set; }

    public uint ExpirationYear { get; set; }
}