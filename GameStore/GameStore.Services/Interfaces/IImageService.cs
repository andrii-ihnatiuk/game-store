using GameStore.Shared.DTOs.Image;
using Microsoft.AspNetCore.Http;

namespace GameStore.Services.Interfaces;

public interface IImageService
{
    Task<IList<ImageBriefDto>> GetAvailableImagesAsync();

    Task UploadImagesAsync(IFormFileCollection files);
}