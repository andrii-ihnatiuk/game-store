using GameStore.Services.Services;
using GameStore.Shared.DTOs.Game;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
[ResponseCache(CacheProfileName = "OneMinuteCache")]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;

    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet("{gameAlias}", Name = "GetGameByAlias")]
    public async Task<IActionResult> GetGameAsync([FromRoute] string gameAlias)
    {
        var gameFullDto = await _gameService.GetGameByAliasAsync(gameAlias);
        return Ok(gameFullDto);
    }

    [HttpGet("")]
    public async Task<ActionResult<GamesWithCountDto>> GetAllGamesAsync()
    {
        var gamesDto = await _gameService.GetAllGamesAsync();
        return Ok(gamesDto);
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
        var gameFullDto = await _gameService.AddGameAsync(dto);
        return CreatedAtRoute("GetGameByAlias", new { gameAlias = gameFullDto.Alias }, gameFullDto);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateGameAsync([FromBody] GameUpdateDto dto)
    {
        await _gameService.UpdateGameAsync(dto);
        return Ok();
    }

    [HttpDelete("remove")]
    public async Task<IActionResult> DeleteGameAsync([FromQuery] long gameId)
    {
        await _gameService.DeleteGameAsync(gameId);
        return NoContent();
    }
}