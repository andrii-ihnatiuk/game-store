using GameStore.Shared.Constants.Filter;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class FiltersController : ControllerBase
{
    [HttpGet("options/date")]
    public IActionResult GetPublishedDateOptionsForGame()
    {
        return Ok(PublishDateOption.AllOptions);
    }

    [HttpGet("options/sorting")]
    public IActionResult GetSortingOptionsForGame()
    {
        return Ok(SortingOption.AllOptions);
    }

    [HttpGet("options/pagination")]
    public IActionResult GetPaginationOptionsForGame()
    {
        return Ok(PaginationOption.AllOptions);
    }
}