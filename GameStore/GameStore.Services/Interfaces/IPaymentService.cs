using GameStore.Shared.DTOs.Order;

namespace GameStore.Services.Interfaces;

public interface IPaymentService
{
    Task<IList<PaymentMethodDto>> GetAvailablePaymentMethodsAsync();

    Task<IPaymentResult> RequestPaymentAsync(PaymentDto payment, Guid customerId);
}