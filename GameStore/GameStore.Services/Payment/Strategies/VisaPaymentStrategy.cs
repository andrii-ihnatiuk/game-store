using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using GameStore.Data.Interfaces;
using GameStore.Services.Interfaces;
using GameStore.Services.Interfaces.Payment;
using GameStore.Services.Models;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Payment.Visa;
using GameStore.Shared.Exceptions;
using GameStore.Shared.Options;
using Microsoft.Extensions.Options;

namespace GameStore.Services.Payment.Strategies;

public class VisaPaymentStrategy : PaymentStrategyBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly VisaOptions _apiOptions;

    public VisaPaymentStrategy(
        IUnitOfWork unitOfWork,
        ICoreOrderService orderService,
        IOptions<VisaOptions> apiSettings,
        IHttpClientFactory httpClientFactory)
        : base(unitOfWork, orderService)
    {
        _httpClientFactory = httpClientFactory;
        _apiOptions = apiSettings.Value;
    }

    public override PaymentStrategyName Name => PaymentStrategyName.Visa;

    protected override async Task<IPaymentResult> DoPaymentAsync(PaymentRequest request)
    {
        using var client = _httpClientFactory.CreateClient();
        var model = request.VisaModel;
        var paymentData = new VisaTransactionRequestDto()
        {
            CardNumber = model.CardNumber,
            CardHolderName = model.Holder,
            ExpirationYear = model.YearExpire,
            ExpirationMonth = model.MonthExpire,
            TransactionAmount = Order.Sum,
            Cvv = model.Cvv2,
        };

        var content = new StringContent(JsonSerializer.Serialize(paymentData), Encoding.UTF8, MediaTypeNames.Application.Json);
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
}