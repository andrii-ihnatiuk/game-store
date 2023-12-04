using GameStore.Application.Interfaces;
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
    private readonly IGenreFacadeService _genreFacadeService;
    private readonly IValidatorWrapper<GenreCreateDto> _genreCreateValidator;
    private readonly IValidatorWrapper<GenreUpdateDto> _genreUpdateValidator;

    public GenresController(
        IGenreFacadeService genreFacadeService,
        IValidatorWrapper<GenreCreateDto> genreCreateValidator,
        IValidatorWrapper<GenreUpdateDto> genreUpdateValidator)
    {
        _genreFacadeService = genreFacadeService;
        _genreCreateValidator = genreCreateValidator;
        _genreUpdateValidator = genreUpdateValidator;
    }

    [HttpGet("{genreId}", Name = "GetGenreById")]
    public async Task<IActionResult> GetGenreAsync([FromRoute] string genreId)
    {
        var genreFullDto = await _genreFacadeService.GetGenreByIdAsync(genreId);
        return Ok(genreFullDto);
    }

    [HttpGet("")]
    public async Task<ActionResult<IList<GenreBriefDto>>> GetAllGenresAsync()
    {
        var genresDto = await _genreFacadeService.GetAllGenresAsync();
        return Ok(genresDto);
    }

    [HttpGet("{id}/subgenres")]
    public async Task<ActionResult<IList<GenreBriefDto>>> GetSubgenresAsync(string id)
    {
        var subgenres = await _genreFacadeService.GetSubgenresByParentAsync(id);
        return Ok(subgenres);
    }

    [HttpGet("{id}/games")]
    public async Task<ActionResult<IList<GameBriefDto>>> GetGamesByGenreAsync(string id)
    {
        var games = await _genreFacadeService.GetGamesByGenreIdAsync(id);
        return Ok(games);
    }

    [HttpPost("new")]
    public async Task<IActionResult> PostGenreAsync([FromBody] GenreCreateDto dto)
    {
        _genreCreateValidator.ValidateAndThrow(dto);
        var genreBriefDto = await _genreFacadeService.AddGenreAsync(dto);
        return CreatedAtRoute("GetGenreById", new { genreId = genreBriefDto.Id }, genreBriefDto);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateGenreAsync([FromBody] GenreUpdateDto dto)
    {
        _genreUpdateValidator.ValidateAndThrow(dto);
        await _genreFacadeService.UpdateGenreAsync(dto);
        return Ok();
    }

    [HttpDelete("remove/{id}")]
    public async Task<IActionResult> DeleteGenreAsync([FromRoute] string id)
    {
        await _genreFacadeService.DeleteGenreAsync(id);
        return NoContent();
    }
}
