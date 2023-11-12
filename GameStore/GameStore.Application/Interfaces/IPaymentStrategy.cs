using GameStore.Shared.DTOs.Payment;

namespace GameStore.Application.Interfaces;

public interface IPaymentStrategy
{
    string Name { get; }

    Task<IPaymentResult> ProcessPayment(PaymentDto payment, Guid customerId);
}