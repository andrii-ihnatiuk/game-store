using System.Text;
using System.Text.Json;
using GameStore.Data.Entities;
using GameStore.Data.Repositories;
using GameStore.Services.Constants;
using GameStore.Services.Exceptions;
using GameStore.Services.Interfaces;
using GameStore.Services.Models;
using GameStore.Shared.DTOs.Order;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services.PaymentStrategies;

public class TerminalPaymentStrategy : IPaymentStrategy
{
    private readonly IUnitOfWork _unitOfWork;

    public TerminalPaymentStrategy(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public string Name => PaymentStrategyName.Terminal;

    public async Task<IPaymentResult> ProcessPayment(PaymentDto payment, Guid customerId)
    {
        var order = await GetOrderForPaymentAsync(customerId);
        var terminalPaymentResult = await SendPaymentRequestAsync(order);
        await UpdateOrderStatusAsync(order);
        return terminalPaymentResult;
    }

    private async Task<Order> GetOrderForPaymentAsync(Guid customerId)
    {
        return await _unitOfWork.Orders.GetOneAsync(
            predicate: o => o.CustomerId == customerId && o.PaidDate == null,
            include: q => q.Include(o => o.OrderDetails),
            noTracking: false);
    }

    private async Task<TerminalPaymentResult> SendPaymentRequestAsync(Order order)
    {
        using var client = new HttpClient();
        var paymentData = new
        {
            TransactionAmount = order.Sum,
            AccountNumber = order.CustomerId,
            InvoiceNumber = order.Id,
        };

        var content = new StringContent(JsonSerializer.Serialize(paymentData), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://localhost:5001/api/payments/ibox", content);
        ThrowPaymentExceptionIfRequestIsNotSuccessful(response);
        return await ConvertResponseToTerminalPaymentResultAsync(response);
    }

    private void ThrowPaymentExceptionIfRequestIsNotSuccessful(HttpResponseMessage responseMessage)
    {
        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new PaymentException($"Couldn't pay using '{Name}'. Either try later or use another payment method.");
        }
    }

    private static async Task<TerminalPaymentResult> ConvertResponseToTerminalPaymentResultAsync(HttpResponseMessage responseMessage)
    {
        string responseString = await responseMessage.Content.ReadAsStringAsync();
        var terminalResponse = JsonSerializer.Deserialize<TerminalResponse>(responseString, new JsonSerializerOptions()
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
        await _unitOfWork.SaveAsync();
    }
}