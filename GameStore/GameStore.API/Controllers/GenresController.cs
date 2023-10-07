using GameStore.Services.Services;
using GameStore.Shared.DTOs.Genre;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpPost("new")]
    public async Task<IActionResult> PostGenreAsync([FromBody] GenreCreateDto dto)
    {
        var genreViewDto = await _genreService.AddGenreAsync(dto);
        return Ok(genreViewDto);
    }
}
