using GameStore.Data.Entities;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PublishersController : ControllerBase
{
    private readonly IPublisherService _publisherService;
    private readonly IValidatorWrapper<PublisherCreateDto> _publisherCreateValidator;

    public PublishersController(
        IPublisherService publisherService,
        IValidatorWrapper<PublisherCreateDto> publisherCreateValidator)
    {
        _publisherService = publisherService;
        _publisherCreateValidator = publisherCreateValidator;
    }

    [HttpGet("{companyName}", Name = "GetPublisherByName")]
    public async Task<ActionResult<PublisherFullDto>> GetPublisherAsync([FromRoute] string companyName)
    {
        var gameFullDto = await _publisherService.GetPublisherByNameAsync(companyName);
        return Ok(gameFullDto);
    }

    [HttpGet]
    public async Task<ActionResult<IList<Publisher>>> GetAllPublishersAsync()
    {
        var publishersDto = await _publisherService.GetAllPublishersAsync();
        return Ok(publishersDto);
    }

    [HttpGet("{companyName}/games")]
    public async Task<ActionResult<IList<GameBriefDto>>> GetGamesByPublisherAsync([FromRoute] string companyName)
    {
        var games = await _publisherService.GetGamesByPublisherNameAsync(companyName);
        return Ok(games);
    }

    [HttpPost("new")]
    public async Task<ActionResult<PublisherBriefDto>> PostPublisherAsync([FromBody] PublisherCreateDto dto)
    {
        _publisherCreateValidator.ValidateAndThrow(dto);
        var publisherBriefDto = await _publisherService.AddPublisherAsync(dto);
        return CreatedAtRoute("GetPublisherByName", new { CompanyName = publisherBriefDto.CompanyName }, publisherBriefDto);
    }
}