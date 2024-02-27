using GameStore.Shared.DTOs.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class LocalizationController : ControllerBase
{
    private readonly RequestLocalizationOptions _localizationOptions;

    public LocalizationController(IOptions<RequestLocalizationOptions> localizationOptions)
    {
        _localizationOptions = localizationOptions.Value;
    }

    [HttpGet("cultures")]
    public IActionResult GetSupportedCultures()
    {
        var cultures = _localizationOptions.SupportedCultures?
            .Select(c => new CultureDto
            {
                Name = c.Name,
                DisplayName = c.DisplayName,
                IsDefault = _localizationOptions.DefaultRequestCulture.Culture.Name == c.Name,
            });

        return Ok(cultures);
    }
}