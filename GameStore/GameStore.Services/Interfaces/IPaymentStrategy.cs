using GameStore.Shared.DTOs.Order;

namespace GameStore.Services.Interfaces;

public interface IPaymentStrategy
{
    string Name { get; }

    Task<IPaymentResult> ProcessPayment(PaymentDto payment, Guid customerId);
}