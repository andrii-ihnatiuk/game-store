using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Services.Interfaces.Payment;
using GameStore.Services.Models;
using GameStore.Shared.DTOs.Order;
using GameStore.Shared.DTOs.Payment;
using GameStore.Shared.Exceptions;

namespace GameStore.Services.Payment;

public class PaymentService : IPaymentService
{
    private readonly IPaymentStrategyResolver _strategyResolver;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PaymentService(IPaymentStrategyResolver strategyResolver, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _strategyResolver = strategyResolver;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IList<PaymentMethodDto>> GetAvailablePaymentMethodsAsync()
    {
        var methods = await _unitOfWork.PaymentMethods.GetAsync();
        return _mapper.Map<IList<PaymentMethodDto>>(methods);
    }

    public async Task<IPaymentResult> RequestPaymentAsync(PaymentDto payment, string customerId)
    {
        PaymentMethod paymentMethod;
        try
        {
            paymentMethod = await _unitOfWork.PaymentMethods.GetOneAsync(p => p.Title.Equals(payment.Method));
            payment.Method = paymentMethod.Id.ToString();
        }
        catch (EntityNotFoundException)
        {
            throw new GameStoreNotSupportedException("Selected payment method is not supported");
        }

        var strategy = _strategyResolver.Resolve(paymentMethod.StrategyName);
        var request = new PaymentRequest
        {
            Method = payment.Method,
            CustomerId = customerId,
            VisaModel = payment.Model,
        };
        return await strategy.ProcessPaymentAsync(request);
    }
}