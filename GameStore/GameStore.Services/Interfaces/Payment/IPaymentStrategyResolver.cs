namespace GameStore.Services.Interfaces.Payment;

public interface IPaymentStrategyResolver
{
    IPaymentStrategy Resolve(string name);
}