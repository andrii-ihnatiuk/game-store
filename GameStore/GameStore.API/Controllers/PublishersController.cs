using GameStore.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PublishersController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Publisher>> GetAllPublishers()
    {
        return Ok(new List<Publisher>());
    }
}