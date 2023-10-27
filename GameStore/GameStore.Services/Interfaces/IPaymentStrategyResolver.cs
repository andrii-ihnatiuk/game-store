namespace GameStore.Services.Interfaces;

public interface IPaymentStrategyResolver
{
    IPaymentStrategy Resolve(string name);
}