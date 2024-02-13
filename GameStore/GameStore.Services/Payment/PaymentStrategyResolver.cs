using GameStore.Services.Interfaces.Payment;
using GameStore.Shared.Constants;
using GameStore.Shared.Exceptions;

namespace GameStore.Services.Payment;

public class PaymentStrategyResolver : IPaymentStrategyResolver
{
    private readonly IEnumerable<IPaymentStrategy> _strategies;

    public PaymentStrategyResolver(IEnumerable<IPaymentStrategy> strategies)
    {
        _strategies = strategies;
    }

    public IPaymentStrategy Resolve(PaymentStrategyName name)
    {
        var strategy = _strategies.FirstOrDefault(s => s.Name == name);
        return strategy ?? throw new PaymentException($"Payment strategy cannot be resolved for '{name}.'");
    }
}