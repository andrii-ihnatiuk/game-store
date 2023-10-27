namespace GameStore.Shared.DTOs.Payment;

public class VisaPaymentDto
{
    public string Holder { get; set; }

    public string CardNumber { get; set; }

    public ushort MonthExpire { get; set; }

    public uint Cvv2 { get; set; }

    public uint YearExpire { get; set; }
}