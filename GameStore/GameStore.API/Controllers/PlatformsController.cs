using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[Route("[controller]")]
[ApiController]
[ResponseCache(CacheProfileName = "OneMinuteCache")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformService _platformService;
    private readonly IValidatorWrapper<PlatformCreateDto> _platformCreateValidator;

    public PlatformsController(IPlatformService platformService, IValidatorWrapper<PlatformCreateDto> platformCreateValidator)
    {
        _platformService = platformService;
        _platformCreateValidator = platformCreateValidator;
    }

    [HttpGet("{platformId:guid}", Name = "GetPlatformById")]
    public async Task<IActionResult> GetPlatformAsync([FromRoute] Guid platformId)
    {
        var viewDto = await _platformService.GetPlatformByIdAsync(platformId);
        return Ok(viewDto);
    }

    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<PlatformBriefDto>>> GetAllPlatformsAsync()
    {
        var platformsDto = await _platformService.GetAllPlatformsAsync();
        return Ok(platformsDto);
    }

    [HttpPost("new")]
    public async Task<IActionResult> PostPlatformAsync([FromBody] PlatformCreateDto dto)
    {
        _platformCreateValidator.ValidateAndThrow(dto);
        var viewDto = await _platformService.AddPlatformAsync(dto);
        return CreatedAtRoute("GetPlatformById", new { platformId = viewDto.Id }, viewDto);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdatePlatformAsync([FromBody] PlatformUpdateDto dto)
    {
        await _platformService.UpdatePlatformAsync(dto);
        return Ok();
    }

    [HttpDelete("remove/{platformId:guid}")]
    public async Task<IActionResult> DeletePlatformAsync([FromRoute] Guid platformId)
    {
        await _platformService.DeletePlatformAsync(platformId);
        return NoContent();
    }
}
