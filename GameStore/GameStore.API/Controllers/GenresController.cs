using GameStore.Services.Interfaces;
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

    public GenresController(IGenreService genreService, IValidatorWrapper<GenreCreateDto> genreCreateValidator)
    {
        _genreService = genreService;
        _genreCreateValidator = genreCreateValidator;
    }

    [HttpGet("{genreId:guid}", Name = "GetGenreById")]
    public async Task<IActionResult> GetGenreAsync([FromRoute] Guid genreId)
    {
        var genreFullDto = await _genreService.GetGenreByIdAsync(genreId);
        return Ok(genreFullDto);
    }

    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<GenreBriefDto>>> GetAllGenresAsync()
    {
        var genresDto = await _genreService.GetAllGenresAsync();
        return Ok(genresDto);
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
        await _genreService.UpdateGenreAsync(dto);
        return Ok();
    }

    [HttpDelete("remove/{genreId:guid}")]
    public async Task<IActionResult> DeleteGenreAsync([FromRoute] Guid genreId)
    {
        await _genreService.DeleteGenreAsync(genreId);
        return NoContent();
    }
}
