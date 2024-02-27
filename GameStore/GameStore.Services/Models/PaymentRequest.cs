using GameStore.Shared.DTOs.Payment;

namespace GameStore.Services.Models;

public class PaymentRequest
{
    public string Method { get; init; }

    public string CustomerId { get; init; }

    public VisaPaymentDto? VisaModel { get; init; }
}