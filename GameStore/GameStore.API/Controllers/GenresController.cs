using GameStore.Services.Services;
using GameStore.Shared.DTOs.Genre;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[Route("[controller]")]
[ApiController]
[ResponseCache(CacheProfileName = "OneMinuteCache")]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
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
        var genreFullDto = await _genreService.AddGenreAsync(dto);
        return CreatedAtRoute("GetGenreById", new { genreId = genreFullDto.Id }, genreFullDto);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateGenreAsync([FromBody] GenreUpdateDto dto)
    {
        await _genreService.UpdateGenreAsync(dto);
        return Ok();
    }

    [HttpDelete("remove")]
    public async Task<IActionResult> DeleteGenreAsync([FromQuery] Guid genreId)
    {
        await _genreService.DeleteGenreAsync(genreId);
        return NoContent();
    }
}
