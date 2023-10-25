using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Order;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private static readonly Guid CustomerId = new("43efd8db-5b4b-4fcf-94d6-7916c7263f43");
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [Route("/cart/buy/{gameAlias}")]
    public async Task<IActionResult> AddGameToCartAsync(string gameAlias)
    {
        await _orderService.AddGameToCartAsync(CustomerId, gameAlias);
        return Ok();
    }

    [HttpGet]
    [Route("/cart")]
    public async Task<ActionResult<IList<OrderDetailDto>>> GetCartByCustomerAsync()
    {
        return Ok(await _orderService.GetCartByCustomerAsync(CustomerId));
    }

    [HttpGet]
    public async Task<ActionResult<OrderBriefDto>> GetPaidOrdersByCustomerAsync()
    {
        return Ok(await _orderService.GetPaidOrdersByCustomerAsync(CustomerId));
    }

    [HttpGet("{orderId:guid}")]
    public async Task<ActionResult<OrderBriefDto>> GetOrderByIdAsync(Guid orderId)
    {
        return Ok(await _orderService.GetOrderByIdAsync(orderId));
    }

    [HttpGet("{orderId:guid}/details")]
    public async Task<ActionResult<IList<OrderDetailDto>>> GetOrderDetailsAsync(Guid orderId)
    {
        return Ok(await _orderService.GetOrderDetailsAsync(orderId));
    }

    [HttpGet]
    [Route("/payment/methods")]
    public async Task<ActionResult<PaymentMethodListDto>> GetAvailablePaymentMethodsAsync()
    {
        var paymentMethodsDto = await _orderService.GetAvailablePaymentMethodsAsync();
        return Ok(new PaymentMethodListDto { PaymentMethods = paymentMethodsDto });
    }

    [HttpDelete]
    [Route("/cart/remove/{gameAlias}")]
    public async Task<IActionResult> DeleteGameFromCartAsync(string gameAlias)
    {
        await _orderService.DeleteGameFromCartAsync(CustomerId, gameAlias);
        return NoContent();
    }
}