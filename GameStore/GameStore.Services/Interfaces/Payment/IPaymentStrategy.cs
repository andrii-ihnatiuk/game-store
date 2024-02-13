using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Payment;

namespace GameStore.Services.Interfaces.Payment;

public interface IPaymentStrategy
{
    PaymentStrategyName Name { get; }

    Task<IPaymentResult> ProcessPaymentAsync(PaymentDto payment, string customerId);
}