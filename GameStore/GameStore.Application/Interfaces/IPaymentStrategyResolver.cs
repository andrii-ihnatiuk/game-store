namespace GameStore.Application.Interfaces;

public interface IPaymentStrategyResolver
{
    IPaymentStrategy Resolve(string name);
}