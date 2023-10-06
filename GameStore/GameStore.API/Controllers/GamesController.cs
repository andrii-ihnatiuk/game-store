using GameStore.Services.Services;
using GameStore.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
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
        var gameViewDto = await _gameService.GetGameByAliasAsync(gameAlias);
        return Ok(gameViewDto);
    }

    [HttpPost("new")]
    public async Task<IActionResult> PostGameAsync([FromBody] GameCreateDto dto)
    {
        var gameViewDto = await _gameService.AddGameAsync(dto);
        return CreatedAtRoute("GetGameByAlias", new { gameAlias = gameViewDto.Alias }, gameViewDto);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateGameAsync([FromBody] GameCreateDto dto)
    {
        await _gameService.UpdateGameAsync(dto);
        return Ok();
    }

    [HttpDelete("remove")]
    public async Task<IActionResult> DeleteAsync([FromQuery] long gameId)
    {
        await _gameService.DeleteGameAsync(gameId);
        return NoContent();
    }
}