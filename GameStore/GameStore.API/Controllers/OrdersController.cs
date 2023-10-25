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
    [Route("/game/{gameAlias}/buy")]
    public async Task<IActionResult> AddGameToCart(string gameAlias)
    {
        await _orderService.AddGameToCartAsync(CustomerId, gameAlias);
        return Ok();
    }

    [HttpGet]
    [Route("/cart")]
    public async Task<ActionResult<IList<CartItemDto>>> GetCustomerCart()
    {
        return Ok(await _orderService.GetCartByCustomerAsync(CustomerId));
    }
}