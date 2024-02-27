using GameStore.Services.Models;
using GameStore.Shared.Constants;

namespace GameStore.Services.Interfaces.Payment;

public interface IPaymentStrategy
{
    PaymentStrategyName Name { get; }

    Task<IPaymentResult> ProcessPaymentAsync(PaymentRequest request);
}