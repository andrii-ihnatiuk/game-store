using GameStore.Services.Services;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
[ResponseCache(CacheProfileName = "OneMinuteCache")]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly IValidatorWrapper<GameCreateDto> _gameCreateValidator;

    public GamesController(IGameService gameService, IValidatorWrapper<GameCreateDto> gameCreateValidator)
    {
        _gameService = gameService;
        _gameCreateValidator = gameCreateValidator;
    }

    [HttpGet("{gameAlias}", Name = "GetGameByAlias")]
    public async Task<IActionResult> GetGameAsync([FromRoute] string gameAlias)
    {
        var gameFullDto = await _gameService.GetGameByAliasAsync(gameAlias);
        return Ok(gameFullDto);
    }

    [HttpGet("")]
    public async Task<ActionResult<IList<GameBriefDto>>> GetAllGamesAsync()
    {
        var gamesDto = await _gameService.GetAllGamesAsync();
        return Ok(gamesDto);
    }

    [HttpGet("{gameAlias}/genres")]
    public async Task<ActionResult<IList<GenreBriefDto>>> GetGenresByGameAlias([FromRoute] string gameAlias)
    {
        var genres = await _gameService.GetGenresByGameAliasAsync(gameAlias);
        return Ok(genres);
    }

    [HttpGet("{gameAlias}/platforms")]
    public async Task<ActionResult<IList<PlatformBriefDto>>> GetPlatformsByGameAlias([FromRoute] string gameAlias)
    {
        var platforms = await _gameService.GetPlatformsByGameAliasAsync(gameAlias);
        return Ok(platforms);
    }

    [HttpGet("{gameAlias}/publisher")]
    public async Task<ActionResult<PublisherBriefDto>> GetPublisherByGameAlias([FromRoute] string gameAlias)
    {
        var publisher = await _gameService.GetPublisherByGameAliasAsync(gameAlias);
        return Ok(publisher);
    }

    [HttpGet("{gameAlias}/download")]
    public async Task<IActionResult> DownloadGameAsync([FromRoute] string gameAlias)
    {
        (byte[] bytes, string fileName) = await _gameService.DownloadAsync(gameAlias);
        return File(bytes, "text/plain", fileDownloadName: fileName);
    }

    [HttpPost("new")]
    public async Task<IActionResult> PostGameAsync([FromBody] GameCreateDto dto)
    {
        _gameCreateValidator.ValidateAndThrow(dto);
        var gameFullDto = await _gameService.AddGameAsync(dto);
        return CreatedAtRoute("GetGameByAlias", new { gameAlias = gameFullDto.Key }, gameFullDto);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateGameAsync([FromBody] GameUpdateDto dto)
    {
        await _gameService.UpdateGameAsync(dto);
        return Ok();
    }

    [HttpDelete("remove")]
    public async Task<IActionResult> DeleteGameAsync([FromQuery] Guid gameId)
    {
        await _gameService.DeleteGameAsync(gameId);
        return NoContent();
    }
}