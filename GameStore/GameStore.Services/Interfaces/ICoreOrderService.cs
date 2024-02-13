using GameStore.Data.Entities;
using GameStore.Shared.DTOs.Order;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Services.Interfaces;

public interface ICoreOrderService : IOrderService
{
    Task AddGameToCartAsync(string customerId, string gameAlias);

    Task<CartDetailsDto> GetCartByCustomerAsync(string customerId);

    Task<Order> GetOrderForProcessingAsync(string customerId, bool noTracking = false);

    Task ShipOrderAsync(string orderId);

    Task DeleteGameFromCartAsync(string customerId, string gameAlias, bool deleteAll);
}