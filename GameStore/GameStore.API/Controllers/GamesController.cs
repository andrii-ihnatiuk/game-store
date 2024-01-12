using GameStore.API.Attributes;
using GameStore.Application.Interfaces;
using GameStore.Data.Extensions;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Authorization;
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
    private readonly IAuthorizationService _authorizationService;

    public GamesController(
        IGameFacadeService gameFacadeService,
        IValidatorWrapper<GameCreateDto> gameCreateValidator,
        IValidatorWrapper<GameUpdateDto> gameUpdateValidator,
        IValidatorWrapper<GamesFilterDto> gamesFilterValidator,
        IAuthorizationService authorizationService)
    {
        _gameFacadeService = gameFacadeService;
        _gameCreateValidator = gameCreateValidator;
        _gameUpdateValidator = gameUpdateValidator;
        _gamesFilterValidator = gamesFilterValidator;
        _authorizationService = authorizationService;
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

    [HttpGet]
    public async Task<ActionResult<IList<GameFullDto>>> GetAllGamesAsync()
    {
        bool showDeleted = User.HasPermission(PermissionOptions.GameViewDeleted) || User.HasPermission(PermissionOptions.GameFull);
        var filtered = await _gameFacadeService.GetFilteredGamesAsync(GamesFilterDto.GameStoreDefaultFilter, showDeleted);
        return Ok(filtered.Games);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<FilteredGamesDto>> GetFilteredGamesAsync([FromQuery] GamesFilterDto filter)
    {
        _gamesFilterValidator.ValidateAndThrow(filter);
        bool showDeleted = User.HasPermission(PermissionOptions.GameViewDeleted) || User.HasPermission(PermissionOptions.GameFull);
        var filtered = await _gameFacadeService.GetFilteredGamesAsync(filter, showDeleted);
        return Ok(filtered);
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

    [HasAnyPermission(PermissionOptions.GameCreate, PermissionOptions.GameFull)]
    [HttpPost("new")]
    public async Task<IActionResult> PostGameAsync([FromBody] GameCreateDto dto)
    {
        _gameCreateValidator.ValidateAndThrow(dto);
        var gameBriefDto = await _gameFacadeService.AddGameAsync(dto);
        return CreatedAtRoute("GetGameByAlias", new { gameAlias = gameBriefDto.Key }, gameBriefDto);
    }

    [HasAnyPermission(PermissionOptions.GameUpdate, PermissionOptions.GameUpdateDeleted, PermissionOptions.GameUpdateOwn, PermissionOptions.GameFull)]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateGameAsync([FromBody] GameUpdateDto dto)
    {
        _gameUpdateValidator.ValidateAndThrow(dto);
        var authResult = await _authorizationService.AuthorizeAsync(User, dto, PolicyNames.CanUpdateGame);
        if (!authResult.Succeeded)
        {
            return Forbid();
        }

        await _gameFacadeService.UpdateGameAsync(dto);
        return Ok();
    }

    [HasAnyPermission(PermissionOptions.GameDelete, PermissionOptions.GameFull)]
    [HttpDelete("remove/{alias}")]
    public async Task<IActionResult> DeleteGameAsync([FromRoute] string alias)
    {
        await _gameFacadeService.DeleteGameAsync(alias);
        return NoContent();
    }
}