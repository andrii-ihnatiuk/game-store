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
using GameStore.Shared.DTOs.Payment.Terminal;
using GameStore.Shared.Exceptions;
using GameStore.Shared.Options;
using Microsoft.Extensions.Options;

namespace GameStore.Services.Payment.Strategies;

public class TerminalPaymentStrategy : IPaymentStrategy
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICoreOrderService _orderService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TerminalOptions _apiOptions;

    public TerminalPaymentStrategy(
        IUnitOfWork unitOfWork,
        ICoreOrderService orderService,
        IOptions<TerminalOptions> apiSettings,
        IHttpClientFactory httpClientFactory)
    {
        _unitOfWork = unitOfWork;
        _orderService = orderService;
        _httpClientFactory = httpClientFactory;
        _apiOptions = apiSettings.Value;
    }

    public PaymentStrategyName Name => PaymentStrategyName.Terminal;

    public async Task<IPaymentResult> ProcessPaymentAsync(PaymentDto payment, string customerId)
    {
        var order = await _orderService.GetOrderForProcessingAsync(customerId);
        order.PaymentMethodId = Guid.Parse(payment.Method);
        var terminalPaymentResult = await SendPaymentRequestAsync(order);
        await UpdateOrderStatusAsync(order);
        return terminalPaymentResult;
    }

    private async Task<TerminalPaymentResult> SendPaymentRequestAsync(Order order)
    {
        using var client = _httpClientFactory.CreateClient();
        var paymentData = new TerminalTransactionRequestDto
        {
            TransactionAmount = order.Sum,
            AccountNumber = order.CustomerId,
            InvoiceNumber = order.Id,
        };

        var content = new StringContent(JsonSerializer.Serialize(paymentData), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(_apiOptions.ApiUrl, content);
        ThrowPaymentExceptionIfRequestIsNotSuccessful(response);
        return await ConvertResponseToTerminalPaymentResultAsync(response);
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

    private static async Task<TerminalPaymentResult> ConvertResponseToTerminalPaymentResultAsync(HttpResponseMessage responseMessage)
    {
        string responseString = await responseMessage.Content.ReadAsStringAsync();
        var terminalResponse = JsonSerializer.Deserialize<TerminalTransactionResponseDto>(responseString, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        });

        return new TerminalPaymentResult
        {
            OrderId = terminalResponse.InvoiceNumber,
            UserId = terminalResponse.AccountNumber,
            Method = terminalResponse.PaymentMethod,
            Sum = terminalResponse.Amount,
        };
    }

    private async Task UpdateOrderStatusAsync(Order order)
    {
        order.PaidDate = DateTime.UtcNow;
        order.Status = OrderStatus.Paid;
        await _unitOfWork.SaveAsync();
    }
}