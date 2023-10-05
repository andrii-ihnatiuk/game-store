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
}