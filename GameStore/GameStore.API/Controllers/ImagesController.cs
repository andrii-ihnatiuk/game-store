using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Image;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ImagesController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImagesController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpGet("available")]
    public async Task<ActionResult<IList<ImageBriefDto>>> GetAvailableImages()
    {
        var images = await _imageService.GetAvailableImagesAsync();
        return Ok(images);
    }

    [HttpPost]
    public async Task<IActionResult> UploadImages([FromForm] IFormFileCollection files)
    {
        await _imageService.UploadImagesAsync(files);
        return NoContent();
    }
}