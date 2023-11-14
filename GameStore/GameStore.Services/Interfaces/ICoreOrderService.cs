using GameStore.Shared.DTOs.Order;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Services.Interfaces;

public interface ICoreOrderService : IOrderService
{
    Task AddGameToCartAsync(string customerId, string gameAlias);

    Task<IList<OrderDetailDto>> GetCartByCustomerAsync(string customerId);

    Task DeleteGameFromCartAsync(string customerId, string gameAlias);
}