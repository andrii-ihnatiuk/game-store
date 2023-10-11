using GameStore.Services.Services;
using GameStore.Shared.DTOs.Platform;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[Route("[controller]")]
[ApiController]
[ResponseCache(CacheProfileName = "OneMinuteCache")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformService _platformService;

    public PlatformsController(IPlatformService platformService)
    {
        _platformService = platformService;
    }

    [HttpGet("{platformId:long}", Name = "GetPlatformById")]
    public async Task<IActionResult> GetPlatformAsync([FromRoute] long platformId)
    {
        var viewDto = await _platformService.GetPlatformByIdAsync(platformId);
        return Ok(viewDto);
    }

    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<PlatformBriefDto>>> GetAllGenresAsync()
    {
        var platformsDto = await _platformService.GetAllPlatformsAsync();
        return Ok(platformsDto);
    }

    [HttpPost("new")]
    public async Task<IActionResult> PostPlatformAsync([FromBody] PlatformCreateDto dto)
    {
        var viewDto = await _platformService.AddPlatformAsync(dto);
        return CreatedAtRoute("GetPlatformById", new { platformId = viewDto.PlatformId }, viewDto);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdatePlatformAsync([FromBody] PlatformUpdateDto dto)
    {
        await _platformService.UpdatePlatformAsync(dto);
        return Ok();
    }

    [HttpDelete("remove")]
    public async Task<IActionResult> DeleteGenreAsync([FromQuery] long platformId)
    {
        await _platformService.DeletePlatformAsync(platformId);
        return NoContent();
    }
}
