﻿using GameStore.Shared.DTOs.Order;
using GameStore.Shared.DTOs.Payment;

namespace GameStore.Services.Interfaces.Payment;

public interface IPaymentService
{
    Task<IList<PaymentMethodDto>> GetAvailablePaymentMethodsAsync();

    Task<IPaymentResult> RequestPaymentAsync(PaymentDto payment, string customerId);
}