using GameStore.Shared.Interfaces.Services;

namespace GameStore.Application.Interfaces;

public interface IOrderServiceProvider
{
    IOrderService GetByIdString(string orderId);

    IEnumerable<IOrderService> GetAll();
}