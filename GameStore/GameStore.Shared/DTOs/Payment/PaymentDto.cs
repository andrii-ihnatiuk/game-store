namespace GameStore.Shared.DTOs.Payment;

public class PaymentDto
{
    public string Method { get; set; }

    public VisaPaymentDto? Model { get; init; }
}