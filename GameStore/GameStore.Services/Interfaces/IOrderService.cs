using GameStore.Shared.DTOs.Order;

namespace GameStore.Services.Interfaces;

public interface IOrderService
{
    Task AddGameToCartAsync(Guid customerId, string gameAlias);

    Task<IList<CartItemDto>> GetCartByCustomerAsync(Guid customerId);
}