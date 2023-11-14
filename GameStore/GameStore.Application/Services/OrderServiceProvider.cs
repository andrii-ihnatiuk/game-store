using GameStore.Application.Interfaces;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Application.Services;

public class OrderServiceProvider : IOrderServiceProvider
{
    private readonly IEnumerable<IOrderService> _orderServices;

    public OrderServiceProvider(IEnumerable<IOrderService> orderServices)
    {
        _orderServices = orderServices;
    }

    public IOrderService GetByIdString(string orderId)
    {
        return Guid.TryParse(orderId, out _)
            ? _orderServices.First(s => s is GameStore.Services.CoreOrderService)
            : _orderServices.First(s => s is Northwind.Services.MongoOrderService);
    }

    public IEnumerable<IOrderService> GetAll() => _orderServices;
}