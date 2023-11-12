using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[Route("[controller]")]
[ApiController]
[ResponseCache(CacheProfileName = "OneMinuteCache")]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;
    private readonly IValidatorWrapper<GenreCreateDto> _genreCreateValidator;
    private readonly IValidatorWrapper<GenreUpdateDto> _genreUpdateValidator;

    public GenresController(
        IGenreService genreService,
        IValidatorWrapper<GenreCreateDto> genreCreateValidator,
        IValidatorWrapper<GenreUpdateDto> genreUpdateValidator)
    {
        _genreService = genreService;
        _genreCreateValidator = genreCreateValidator;
        _genreUpdateValidator = genreUpdateValidator;
    }

    [HttpGet("{genreId:guid}", Name = "GetGenreById")]
    public async Task<IActionResult> GetGenreAsync([FromRoute] Guid genreId)
    {
        var genreFullDto = await _genreService.GetGenreByIdAsync(genreId);
        return Ok(genreFullDto);
    }

    [HttpGet("")]
    public async Task<ActionResult<IList<GenreBriefDto>>> GetAllGenresAsync()
    {
        var genresDto = await _genreService.GetAllGenresAsync();
        return Ok(genresDto);
    }

    [HttpGet("{id:guid}/subgenres")]
    public async Task<ActionResult<IList<GenreBriefDto>>> GetSubgenresAsync(Guid id)
    {
        var subgenres = await _genreService.GetSubgenresByParentAsync(id);
        return Ok(subgenres);
    }

    [HttpGet("{id:guid}/games")]
    public async Task<ActionResult<IList<GameBriefDto>>> GetGamesByGenreAsync(Guid id)
    {
        var games = await _genreService.GetGamesByGenreId(id);
        return Ok(games);
    }

    [HttpPost("new")]
    public async Task<IActionResult> PostGenreAsync([FromBody] GenreCreateDto dto)
    {
        _genreCreateValidator.ValidateAndThrow(dto);
        var genreBriefDto = await _genreService.AddGenreAsync(dto);
        return CreatedAtRoute("GetGenreById", new { genreId = genreBriefDto.Id }, genreBriefDto);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateGenreAsync([FromBody] GenreUpdateDto dto)
    {
        _genreUpdateValidator.ValidateAndThrow(dto);
        await _genreService.UpdateGenreAsync(dto);
        return Ok();
    }

    [HttpDelete("remove/{id:guid}")]
    public async Task<IActionResult> DeleteGenreAsync([FromRoute] Guid id)
    {
        await _genreService.DeleteGenreAsync(id);
        return NoContent();
    }
}
