using GameStore.API.Attributes;
using GameStore.Application.Interfaces;
using GameStore.Data.Entities;
using GameStore.Data.Extensions;
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

    [HttpGet("{id}", Name = "GetPublisherById")]
    public async Task<ActionResult<PublisherFullDto>> GetPublisherAsync([FromRoute] string id)
    {
        var gameFullDto = await _publisherFacadeService.GetPublisherByIdAsync(id, this.GetCurrentCultureName());
        return Ok(gameFullDto);
    }

    [HttpGet]
    public async Task<ActionResult<IList<Publisher>>> GetAllPublishersAsync()
    {
        var publishersDto = await _publisherFacadeService.GetAllPublishersAsync(this.GetCurrentCultureName());
        return Ok(publishersDto);
    }

    [HttpGet("{id}/games")]
    public async Task<ActionResult<IList<GameBriefDto>>> GetGamesByPublisherAsync([FromRoute] string id)
    {
        var games = await _publisherFacadeService.GetGamesByPublisherIdAsync(id, this.GetCurrentCultureName());
        return Ok(games);
    }

    [HasAnyPermission(PermissionOptions.PublisherCreate, PermissionOptions.PublisherFull)]
    [HttpPost("new")]
    public async Task<ActionResult<PublisherBriefDto>> PostPublisherAsync([FromBody] PublisherCreateDto dto)
    {
        _publisherCreateValidator.ValidateAndThrow(dto);
        var publisherBriefDto = await _publisherFacadeService.AddPublisherAsync(dto);
        return CreatedAtRoute("GetPublisherById", new { id = publisherBriefDto.Id }, publisherBriefDto);
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