using System.Globalization;
using System.Text.RegularExpressions;
using GameStore.Application.Interfaces;
using GameStore.Services.Interfaces;
using GameStore.Services.Interfaces.Payment;
using GameStore.Services.Models;
using GameStore.Shared.DTOs.Order;
using GameStore.Shared.DTOs.Payment;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private static readonly string CustomerId = "VINET";
    private readonly ICoreOrderService _coreOrderService;
    private readonly IOrderFacadeService _orderFacadeService;
    private readonly IPaymentService _paymentService;
    private readonly IValidatorWrapper<PaymentDto> _paymentValidator;

    public OrdersController(
        ICoreOrderService coreOrderService,
        IOrderFacadeService orderFacadeService,
        IPaymentService paymentService,
        IValidatorWrapper<PaymentDto> paymentValidator)
    {
        _coreOrderService = coreOrderService;
        _orderFacadeService = orderFacadeService;
        _paymentService = paymentService;
        _paymentValidator = paymentValidator;
    }

    [HttpGet]
    [Route("/cart/buy/{gameAlias}")]
    public async Task<IActionResult> AddGameToCartAsync(string gameAlias)
    {
        await _coreOrderService.AddGameToCartAsync(CustomerId, gameAlias);
        return Ok();
    }

    [HttpGet]
    [Route("/cart")]
    public async Task<ActionResult<IList<OrderDetailDto>>> GetCartByCustomerAsync()
    {
        return Ok(await _coreOrderService.GetCartByCustomerAsync(CustomerId));
    }

    [HttpGet("history")]
    public async Task<ActionResult<IList<OrderBriefDto>>> GetOrdersHistoryByCustomerAsync(string? start = null, string? end = null)
    {
        var lowerDate = start is null ? DateTime.MinValue : GetDateTimeFromString(start);
        var upperDate = end is null ? DateTime.MaxValue : GetDateTimeFromString(end);
        return Ok(await _orderFacadeService.GetOrdersHistoryByCustomerAsync(CustomerId, lowerDate, upperDate));
    }

    [HttpGet("{orderId}")]
    public async Task<ActionResult<OrderBriefDto>> GetOrderByIdAsync(string orderId)
    {
        return Ok(await _orderFacadeService.GetOrderByIdAsync(orderId));
    }

    [HttpGet("{orderId}/details")]
    public async Task<ActionResult<IList<OrderDetailDto>>> GetOrderDetailsAsync(string orderId)
    {
        return Ok(await _orderFacadeService.GetOrderDetailsAsync(orderId));
    }

    [HttpGet]
    [Route("/payment/methods")]
    public async Task<ActionResult<PaymentMethodListDto>> GetAvailablePaymentMethodsAsync()
    {
        var paymentMethodsDto = await _paymentService.GetAvailablePaymentMethodsAsync();
        return Ok(new PaymentMethodListDto { PaymentMethods = paymentMethodsDto });
    }

    [HttpPost("pay")]
    public async Task<IActionResult> PayForOrderAsync(PaymentDto payment)
    {
        _paymentValidator.ValidateAndThrow(payment);
        var paymentResult = await _paymentService.RequestPaymentAsync(payment, CustomerId);
        IActionResult actionResult = paymentResult switch
        {
            BankPaymentResult result => File(result.InvoiceFileBytes, result.ContentType, result.FileDownloadName),
            TerminalPaymentResult result => Ok(result),
            VisaPaymentResult result => Ok(result),
            _ => BadRequest(),
        };
        return actionResult;
    }

    [HttpDelete]
    [Route("/cart/remove/{gameAlias}")]
    public async Task<IActionResult> DeleteGameFromCartAsync(string gameAlias)
    {
        await _coreOrderService.DeleteGameFromCartAsync(CustomerId, gameAlias);
        return NoContent();
    }

    private static DateTime GetDateTimeFromString(string dateString)
    {
        dateString = dateString[..dateString.IndexOf(" (", StringComparison.Ordinal)];
        dateString = Regex.Replace(dateString, @"GMT\s", "GMT+");
        const string format = "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz";
        var dateTimeOffset = DateTimeOffset.ParseExact(dateString, format, CultureInfo.InvariantCulture);
        return dateTimeOffset.DateTime;
    }
}