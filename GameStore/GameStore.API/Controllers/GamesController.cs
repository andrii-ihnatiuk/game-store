using GameStore.Application.Interfaces;
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
    private readonly IGameFacadeService _gameFacadeService;
    private readonly IValidatorWrapper<GameCreateDto> _gameCreateValidator;
    private readonly IValidatorWrapper<GameUpdateDto> _gameUpdateValidator;
    private readonly IValidatorWrapper<GamesFilterDto> _gamesFilterValidator;

    public GamesController(
        IGameFacadeService gameFacadeService,
        IValidatorWrapper<GameCreateDto> gameCreateValidator,
        IValidatorWrapper<GameUpdateDto> gameUpdateValidator,
        IValidatorWrapper<GamesFilterDto> gamesFilterValidator)
    {
        _gameFacadeService = gameFacadeService;
        _gameCreateValidator = gameCreateValidator;
        _gameUpdateValidator = gameUpdateValidator;
        _gamesFilterValidator = gamesFilterValidator;
    }

    [HttpGet("{gameAlias}", Name = "GetGameByAlias")]
    public async Task<IActionResult> GetGameByAliasAsync([FromRoute] string gameAlias)
    {
        var gameFullDto = await _gameFacadeService.GetGameByAliasAsync(gameAlias);
        return Ok(gameFullDto);
    }

    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetGameByIdAsync([FromRoute] string id)
    {
        var gameFullDto = await _gameFacadeService.GetGameByIdAsync(id);
        return Ok(gameFullDto);
    }

    [HttpGet("")]
    public async Task<ActionResult<FilteredGamesDto>> GetAllGamesAsync([FromQuery] GamesFilterDto filter)
    {
        _gamesFilterValidator.ValidateAndThrow(filter);
        var gamesDto = await _gameFacadeService.GetAllGamesAsync(filter);
        return Ok(gamesDto);
    }

    [HttpGet("{gameAlias}/genres")]
    [ResponseCache(CacheProfileName = "OneMinuteCache")]
    public async Task<ActionResult<IList<GenreBriefDto>>> GetGenresByGameAliasAsync([FromRoute] string gameAlias)
    {
        var genres = await _gameFacadeService.GetGenresByGameAliasAsync(gameAlias);
        return Ok(genres);
    }

    [HttpGet("{gameAlias}/platforms")]
    [ResponseCache(CacheProfileName = "OneMinuteCache")]
    public async Task<ActionResult<IList<PlatformBriefDto>>> GetPlatformsByGameAliasAsync([FromRoute] string gameAlias)
    {
        var platforms = await _gameFacadeService.GetPlatformsByGameAliasAsync(gameAlias);
        return Ok(platforms);
    }

    [HttpGet("{gameAlias}/publisher")]
    [ResponseCache(CacheProfileName = "OneMinuteCache")]
    public async Task<ActionResult<PublisherBriefDto>> GetPublisherByGameAliasAsync([FromRoute] string gameAlias)
    {
        var publisher = await _gameFacadeService.GetPublisherByGameAliasAsync(gameAlias);
        return Ok(publisher);
    }

    [HttpGet("{gameAlias}/download")]
    [ResponseCache(CacheProfileName = "OneMinuteCache")]
    public async Task<IActionResult> DownloadGameAsync([FromRoute] string gameAlias)
    {
        (byte[] bytes, string fileName) = await _gameFacadeService.DownloadAsync(gameAlias);
        return File(bytes, "text/plain", fileDownloadName: fileName);
    }

    [HttpPost("new")]
    public async Task<IActionResult> PostGameAsync([FromBody] GameCreateDto dto)
    {
        _gameCreateValidator.ValidateAndThrow(dto);
        var gameBriefDto = await _gameFacadeService.AddGameAsync(dto);
        return CreatedAtRoute("GetGameByAlias", new { gameAlias = gameBriefDto.Key }, gameBriefDto);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateGameAsync([FromBody] GameUpdateDto dto)
    {
        _gameUpdateValidator.ValidateAndThrow(dto);
        await _gameFacadeService.UpdateGameAsync(dto);
        return Ok();
    }

    [HttpDelete("remove/{alias}")]
    public async Task<IActionResult> DeleteGameAsync([FromRoute] string alias)
    {
        await _gameFacadeService.DeleteGameAsync(alias);
        return NoContent();
    }
}