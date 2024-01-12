using GameStore.API.Attributes;
using GameStore.Application.Interfaces;
using GameStore.Data.Entities;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PublishersController : ControllerBase
{
    private readonly IPublisherFacadeService _publisherFacadeService;
    private readonly IValidatorWrapper<PublisherCreateDto> _publisherCreateValidator;
    private readonly IValidatorWrapper<PublisherUpdateDto> _publisherUpdateValidator;
    private readonly IAuthorizationService _authorizationService;

    public PublishersController(
        IPublisherFacadeService publisherFacadeService,
        IValidatorWrapper<PublisherCreateDto> publisherCreateValidator,
        IValidatorWrapper<PublisherUpdateDto> publisherUpdateValidator,
        IAuthorizationService authorizationService)
    {
        _publisherFacadeService = publisherFacadeService;
        _publisherCreateValidator = publisherCreateValidator;
        _publisherUpdateValidator = publisherUpdateValidator;
        _authorizationService = authorizationService;
    }

    [HttpGet("{companyName}", Name = "GetPublisherByName")]
    public async Task<ActionResult<PublisherFullDto>> GetPublisherAsync([FromRoute] string companyName)
    {
        var gameFullDto = await _publisherFacadeService.GetPublisherByNameAsync(companyName);
        return Ok(gameFullDto);
    }

    [HttpGet]
    public async Task<ActionResult<IList<Publisher>>> GetAllPublishersAsync()
    {
        var publishersDto = await _publisherFacadeService.GetAllPublishersAsync();
        return Ok(publishersDto);
    }

    [HttpGet("{companyName}/games")]
    public async Task<ActionResult<IList<GameBriefDto>>> GetGamesByPublisherAsync([FromRoute] string companyName)
    {
        var games = await _publisherFacadeService.GetGamesByPublisherNameAsync(companyName);
        return Ok(games);
    }

    [HasAnyPermission(PermissionOptions.PublisherCreate, PermissionOptions.PublisherFull)]
    [HttpPost("new")]
    public async Task<ActionResult<PublisherBriefDto>> PostPublisherAsync([FromBody] PublisherCreateDto dto)
    {
        _publisherCreateValidator.ValidateAndThrow(dto);
        var publisherBriefDto = await _publisherFacadeService.AddPublisherAsync(dto);
        return CreatedAtRoute("GetPublisherByName", new { CompanyName = publisherBriefDto.CompanyName }, publisherBriefDto);
    }

    [HasAnyPermission(PermissionOptions.PublisherUpdate, PermissionOptions.PublisherUpdateSelf, PermissionOptions.PublisherFull)]
    [HttpPut("update")]
    public async Task<IActionResult> UpdatePublisherAsync([FromBody] PublisherUpdateDto dto)
    {
        _publisherUpdateValidator.ValidateAndThrow(dto);
        var authResult = await _authorizationService.AuthorizeAsync(User, dto, PolicyNames.CanUpdatePublisher);
        if (!authResult.Succeeded)
        {
            return Forbid();
        }

        await _publisherFacadeService.UpdatePublisherAsync(dto);
        return Ok();
    }

    [HasAnyPermission(PermissionOptions.PublisherDelete, PermissionOptions.PublisherFull)]
    [HttpDelete("remove/{id}")]
    public async Task<IActionResult> DeletePublisherAsync([FromRoute] string id)
    {
        await _publisherFacadeService.DeletePublisherAsync(id);
        return NoContent();
    }
}