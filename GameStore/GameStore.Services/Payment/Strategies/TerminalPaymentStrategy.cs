using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using GameStore.Data.Interfaces;
using GameStore.Services.Interfaces;
using GameStore.Services.Interfaces.Payment;
using GameStore.Services.Models;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Payment.Terminal;
using GameStore.Shared.Exceptions;
using GameStore.Shared.Options;
using Microsoft.Extensions.Options;

namespace GameStore.Services.Payment.Strategies;

public class TerminalPaymentStrategy : PaymentStrategyBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TerminalOptions _apiOptions;

    public TerminalPaymentStrategy(
        IUnitOfWork unitOfWork,
        ICoreOrderService orderService,
        IOptions<TerminalOptions> apiSettings,
        IHttpClientFactory httpClientFactory)
        : base(unitOfWork, orderService)
    {
        _httpClientFactory = httpClientFactory;
        _apiOptions = apiSettings.Value;
    }

    public override PaymentStrategyName Name => PaymentStrategyName.Terminal;

    protected override async Task<IPaymentResult> DoPaymentAsync(PaymentRequest request)
    {
        using var client = _httpClientFactory.CreateClient();
        var paymentData = new TerminalTransactionRequestDto
        {
            TransactionAmount = Order.Sum,
            AccountNumber = Order.CustomerId,
            InvoiceNumber = Order.Id,
        };

        var content = new StringContent(JsonSerializer.Serialize(paymentData), Encoding.UTF8, MediaTypeNames.Application.Json);
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
}