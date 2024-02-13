using System.Net;
using System.Text;
using System.Text.Json;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Services.Interfaces;
using GameStore.Services.Interfaces.Payment;
using GameStore.Services.Models;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Payment;
using GameStore.Shared.DTOs.Payment.Visa;
using GameStore.Shared.Exceptions;
using GameStore.Shared.Options;
using Microsoft.Extensions.Options;

namespace GameStore.Services.Payment.Strategies;

public class VisaPaymentStrategy : IPaymentStrategy
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICoreOrderService _orderService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly VisaOptions _apiOptions;

    public VisaPaymentStrategy(
        IUnitOfWork unitOfWork,
        ICoreOrderService orderService,
        IOptions<VisaOptions> apiSettings,
        IHttpClientFactory httpClientFactory)
    {
        _unitOfWork = unitOfWork;
        _orderService = orderService;
        _httpClientFactory = httpClientFactory;
        _apiOptions = apiSettings.Value;
    }

    public PaymentStrategyName Name => PaymentStrategyName.Visa;

    public async Task<IPaymentResult> ProcessPaymentAsync(PaymentDto payment, string customerId)
    {
        var order = await _orderService.GetOrderForProcessingAsync(customerId);
        order.PaymentMethodId = Guid.Parse(payment.Method);
        var visaPaymentResult = await SendPaymentRequestAsync(payment, order);
        await UpdateOrderStatusAsync(order);
        return visaPaymentResult;
    }

    private async Task<VisaPaymentResult> SendPaymentRequestAsync(PaymentDto payment, Order order)
    {
        using var client = _httpClientFactory.CreateClient();
        var paymentData = new VisaTransactionRequestDto()
        {
            CardNumber = payment.Model.CardNumber,
            CardHolderName = payment.Model.Holder,
            ExpirationYear = payment.Model.YearExpire,
            ExpirationMonth = payment.Model.MonthExpire,
            TransactionAmount = order.Sum,
            Cvv = payment.Model.Cvv2,
        };

        var content = new StringContent(JsonSerializer.Serialize(paymentData), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(_apiOptions.ApiUrl, content);
        ThrowPaymentExceptionIfRequestIsNotSuccessful(response);
        return new VisaPaymentResult();
    }

    private void ThrowPaymentExceptionIfRequestIsNotSuccessful(HttpResponseMessage responseMessage)
    {
        if (responseMessage.IsSuccessStatusCode)
        {
            return;
        }

        var message = $"Couldn't pay using '{Name}'. Either try later or use another payment method.";
        if (responseMessage.StatusCode == HttpStatusCode.PaymentRequired)
        {
            message = "Not enough money on the balance!";
        }

        throw new PaymentException(message);
    }

    private async Task UpdateOrderStatusAsync(Order order)
    {
        order.PaidDate = DateTime.UtcNow;
        order.Status = OrderStatus.Paid;
        await _unitOfWork.SaveAsync();
    }
}