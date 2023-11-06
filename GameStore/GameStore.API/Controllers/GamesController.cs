using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly IValidatorWrapper<GameCreateDto> _gameCreateValidator;
    private readonly IValidatorWrapper<GameUpdateDto> _gameUpdateValidator;

    public GamesController(
        IGameService gameService,
        IValidatorWrapper<GameCreateDto> gameCreateValidator,
        IValidatorWrapper<GameUpdateDto> gameUpdateValidator)
    {
        _gameService = gameService;
        _gameCreateValidator = gameCreateValidator;
        _gameUpdateValidator = gameUpdateValidator;
    }

    [HttpGet("{gameAlias}", Name = "GetGameByAlias")]
    public async Task<IActionResult> GetGameByAliasAsync([FromRoute] string gameAlias)
    {
        var gameFullDto = await _gameService.GetGameByAliasAsync(gameAlias);
        return Ok(gameFullDto);
    }

    [HttpGet("id/{id:guid}")]
    public async Task<IActionResult> GetGameByIdAsync([FromRoute] Guid id)
    {
        var gameFullDto = await _gameService.GetGameByIdAsync(id);
        return Ok(gameFullDto);
    }

    [HttpGet("")]
    public async Task<ActionResult<FilteredGamesDto>> GetAllGamesAsync([FromQuery] GamesFilterOptions filterOptions)
    {
        var gamesDto = await _gameService.GetAllGamesAsync(filterOptions);
        return Ok(gamesDto);
    }

    [HttpGet("{gameAlias}/genres")]
    [ResponseCache(CacheProfileName = "OneMinuteCache")]
    public async Task<ActionResult<IList<GenreBriefDto>>> GetGenresByGameAliasAsync([FromRoute] string gameAlias)
    {
        var genres = await _gameService.GetGenresByGameAliasAsync(gameAlias);
        return Ok(genres);
    }

    [HttpGet("{gameAlias}/platforms")]
    [ResponseCache(CacheProfileName = "OneMinuteCache")]
    public async Task<ActionResult<IList<PlatformBriefDto>>> GetPlatformsByGameAliasAsync([FromRoute] string gameAlias)
    {
        var platforms = await _gameService.GetPlatformsByGameAliasAsync(gameAlias);
        return Ok(platforms);
    }

    [HttpGet("{gameAlias}/publisher")]
    [ResponseCache(CacheProfileName = "OneMinuteCache")]
    public async Task<ActionResult<PublisherBriefDto>> GetPublisherByGameAliasAsync([FromRoute] string gameAlias)
    {
        var publisher = await _gameService.GetPublisherByGameAliasAsync(gameAlias);
        return Ok(publisher);
    }

    [HttpGet("{gameAlias}/download")]
    [ResponseCache(CacheProfileName = "OneMinuteCache")]
    public async Task<IActionResult> DownloadGameAsync([FromRoute] string gameAlias)
    {
        (byte[] bytes, string fileName) = await _gameService.DownloadAsync(gameAlias);
        return File(bytes, "text/plain", fileDownloadName: fileName);
    }

    [HttpPost("new")]
    public async Task<IActionResult> PostGameAsync([FromBody] GameCreateDto dto)
    {
        _gameCreateValidator.ValidateAndThrow(dto);
        var gameBriefDto = await _gameService.AddGameAsync(dto);
        return CreatedAtRoute("GetGameByAlias", new { gameAlias = gameBriefDto.Key }, gameBriefDto);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateGameAsync([FromBody] GameUpdateDto dto)
    {
        _gameUpdateValidator.ValidateAndThrow(dto);
        await _gameService.UpdateGameAsync(dto);
        return Ok();
    }

    [HttpDelete("remove/{alias}")]
    public async Task<IActionResult> DeleteGameAsync([FromRoute] string alias)
    {
        await _gameService.DeleteGameAsync(alias);
        return NoContent();
    }
}