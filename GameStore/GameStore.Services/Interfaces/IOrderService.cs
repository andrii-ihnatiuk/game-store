using GameStore.Shared.DTOs.Order;

namespace GameStore.Services.Interfaces;

public interface IOrderService
{
    Task AddGameToCartAsync(Guid customerId, string gameAlias);

    Task<IList<OrderDetailDto>> GetCartByCustomerAsync(Guid customerId);

    Task<IList<OrderBriefDto>> GetPaidOrdersByCustomerAsync(Guid customerId);

    Task<OrderBriefDto> GetOrderByIdAsync(Guid orderId);

    Task<IList<OrderDetailDto>> GetOrderDetailsAsync(Guid orderId);

    Task<IList<PaymentMethodDto>> GetAvailablePaymentMethodsAsync();

    Task<IPaymentResult> RequestPaymentAsync(PaymentDto payment, Guid customerId);

    Task DeleteGameFromCartAsync(Guid customerId, string gameAlias);
}