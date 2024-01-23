using System.Data;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Services.Interfaces;
using GameStore.Services.Interfaces.Payment;
using GameStore.Services.Models;
using GameStore.Shared.Constants;

namespace GameStore.Services.Payment.Strategies;

public abstract class PaymentStrategyBase : IPaymentStrategy
{
    public PaymentStrategyBase(IUnitOfWork unitOfWork, ICoreOrderService orderService)
    {
        UnitOfWork = unitOfWork;
        OrderService = orderService;
    }

    public abstract PaymentStrategyName Name { get; }

    protected IUnitOfWork UnitOfWork { get; }

    protected ICoreOrderService OrderService { get; }

    protected Order Order { get; set; }

    public async Task<IPaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        await using var transaction = await UnitOfWork.BeginTransactionAsync(IsolationLevel.RepeatableRead);
        try
        {
            await PrepareDataAsync(request);
            var paymentResult = await DoPaymentAsync(request);
            await CompletePaymentAsync();

            await transaction.CommitAsync();
            return paymentResult;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    protected virtual async Task PrepareDataAsync(PaymentRequest request)
    {
        Order = await OrderService.GetOrderForProcessingAsync(request.CustomerId);
        Order.PaymentMethodId = Guid.Parse(request.Method);
    }

    protected abstract Task<IPaymentResult> DoPaymentAsync(PaymentRequest request);

    protected virtual async Task CompletePaymentAsync()
    {
        await OrderService.UpdateOrderStatusAsync(Order.Id.ToString(), OrderStatus.Paid);
        UpdateQuantitiesForProducts(Order.OrderDetails);
        await UnitOfWork.SaveAsync();
    }

    protected static void UpdateQuantitiesForProducts(IEnumerable<OrderDetail> orderDetails)
    {
        foreach (var detail in orderDetails)
        {
            detail.Product.UnitInStock = (short)(detail.Product.UnitInStock - detail.Quantity);
        }
    }
}