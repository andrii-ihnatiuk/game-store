using GameStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotificationMethod()
    {
        var methods = await _notificationService.GetNotificationMethodsAsync();
        return Ok(methods);
    }

    [HttpPost]
    public IActionResult SendNotification()
    {
        return NoContent();
    }
}