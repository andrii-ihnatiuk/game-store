using System.Globalization;
using System.Security.Claims;
using System.Text.RegularExpressions;
using GameStore.API.Attributes;
using GameStore.Application.Interfaces;
using GameStore.Services.Interfaces;
using GameStore.Services.Interfaces.Payment;
using GameStore.Services.Models;
using GameStore.Shared.Constants;
using GameStore.Shared.Constants.Filter;
using GameStore.Shared.DTOs.Order;
using GameStore.Shared.DTOs.Payment;
using GameStore.Shared.Exceptions;
using GameStore.Shared.Models;
using GameStore.Shared.Util;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
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

    [Authorize]
    [HttpGet]
    [Route("/cart")]
    public async Task<ActionResult<CartDetailsDto>> GetCartByCustomerAsync()
    {
        string customerId = User.Claims.Single(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
        return Ok(await _coreOrderService.GetCartByCustomerAsync(customerId));
    }

    [HasAnyPermission(PermissionOptions.OrderViewActive, PermissionOptions.OrderFull)]
    [HttpGet]
    public async Task<ActionResult<IList<OrderBriefDto>>> GetActiveOrdersAsync()
    {
        var filter = new OrdersFilter(DateTime.UtcNow.AddDays(-30), DateTime.MaxValue)
        {
            OnlyPaid = true,
            Stores = new List<string> { StoreOption.GameStore },
        };
        var orders = await _orderFacadeService.GetFilteredOrdersAsync(filter);
        return Ok(orders);
    }

    [HasAnyPermission(PermissionOptions.OrderViewHistory, PermissionOptions.OrderFull)]
    [HttpGet("history")]
    public async Task<ActionResult<IList<OrderBriefDto>>> GetOrdersHistoryAsync(string? start = null, string? end = null)
    {
        var lowerDate = start is null ? DateTime.MinValue : GetDateTimeFromString(start);
        var upperDate = end is null ? DateTime.UtcNow.AddDays(-30) : GetDateTimeFromString(end);
        var filter = new OrdersFilter(lowerDate, upperDate);
        return Ok(await _orderFacadeService.GetFilteredOrdersAsync(filter));
    }

    [HasAnyPermission(PermissionOptions.OrderViewActive, PermissionOptions.OrderFull)]
    [HttpGet("{orderId}")]
    public async Task<ActionResult<OrderBriefDto>> GetOrderByIdAsync(string orderId)
    {
        return Ok(await _orderFacadeService.GetOrderByIdAsync(orderId));
    }

    [HasAnyPermission(PermissionOptions.OrderViewActive, PermissionOptions.OrderFull)]
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

    [Authorize]
    [HttpPost]
    [Route("/cart/buy/{gameAlias}")]
    public async Task<IActionResult> AddGameToCartAsync(string gameAlias)
    {
        if (EntityAliasUtil.ContainsSuffix(gameAlias))
        {
            throw new GameStoreNotSupportedException("Ordering goods from Northwind is not supported!");
        }

        string customerId = User.Claims.Single(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
        await _coreOrderService.AddGameToCartAsync(customerId, gameAlias);
        return Ok();
    }

    [Authorize]
    [HttpPost("pay")]
    public async Task<IActionResult> PayForOrderAsync(PaymentDto payment)
    {
        _paymentValidator.ValidateAndThrow(payment);
        string customerId = User.Claims.Single(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
        var paymentResult = await _paymentService.RequestPaymentAsync(payment, customerId);
        IActionResult actionResult = paymentResult switch
        {
            BankPaymentResult result => File(result.InvoiceFileBytes, result.ContentType, result.FileDownloadName),
            TerminalPaymentResult result => Ok(result),
            VisaPaymentResult result => Ok(result),
            _ => BadRequest(),
        };
        return actionResult;
    }

    [HasAnyPermission(PermissionOptions.OrderUpdate, PermissionOptions.OrderFull)]
    [HttpPost("{orderId}/ship")]
    public async Task<IActionResult> ShipOrderAsync(string orderId)
    {
        await _orderFacadeService.ShipOrderAsync(orderId);
        return NoContent();
    }

    [Authorize]
    [HttpDelete]
    [Route("/cart/remove/{gameAlias}")]
    public async Task<IActionResult> DeleteGameFromCartAsync([FromRoute] string gameAlias, [FromQuery] bool deleteAll = false)
    {
        string customerId = User.Claims.Single(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
        await _coreOrderService.DeleteGameFromCartAsync(customerId, gameAlias, deleteAll);
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