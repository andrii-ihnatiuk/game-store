namespace GameStore.Shared.DTOs.Payment;

public class PaymentDto
{
    public string Method { get; init; }

    public VisaPaymentDto? Model { get; init; }
}