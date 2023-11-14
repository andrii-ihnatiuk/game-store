using AutoMapper;
using GameStore.Data.Interfaces;
using GameStore.Services.Interfaces.Payment;
using GameStore.Shared.DTOs.Order;
using GameStore.Shared.DTOs.Payment;

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

    public Task<IPaymentResult> RequestPaymentAsync(PaymentDto payment, string customerId)
    {
        var strategy = _strategyResolver.Resolve(payment.Method);
        return strategy.ProcessPayment(payment, customerId);
    }
}