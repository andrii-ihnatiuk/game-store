using GameStore.Shared.Constants;

namespace GameStore.Services.Interfaces.Payment;

public interface IPaymentStrategyResolver
{
    IPaymentStrategy Resolve(PaymentStrategyName name);
}