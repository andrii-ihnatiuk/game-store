using GameStore.Shared.DTOs.Payment;

namespace GameStore.Services.Interfaces;

public interface IPaymentStrategy
{
    string Name { get; }

    Task<IPaymentResult> ProcessPayment(PaymentDto payment, Guid customerId);
}