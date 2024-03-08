using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Services.Interfaces;
using GameStore.Services.Interfaces.Util;
using GameStore.Shared.DTOs.Image;
using GameStore.Shared.Exceptions;
using GameStore.Shared.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace GameStore.Services;

public class ImageService : IImageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobFileService _blobService;
    private readonly IMapper _mapper;
    private readonly FileUploadOptions _fileOptions;

    public ImageService(
        IUnitOfWork unitOfWork,
        IBlobFileService blobService,
        IMapper mapper,
        IOptions<FileUploadOptions> fileOptions)
    {
        _unitOfWork = unitOfWork;
        _blobService = blobService;
        _mapper = mapper;
        _fileOptions = fileOptions.Value;
    }

    public async Task UploadImagesAsync(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            ValidateFile(file);
            using var fileStream = file.OpenReadStream();
            using Stream largeImageStream = new MemoryStream();
            using Stream smallImageStream = new MemoryStream();

            try
            {
                using var image = await Image.LoadAsync(fileStream);
                ResizeImageIfBiggerThanMax(image, 1200, 1200);
                await image.SaveAsync(largeImageStream, image.Metadata.DecodedImageFormat!);
                ResizeImageIfBiggerThanMax(image, 256, 256);
                await image.SaveAsync(smallImageStream, image.Metadata.DecodedImageFormat!);

                largeImageStream.Seek(0, SeekOrigin.Begin);
                smallImageStream.Seek(0, SeekOrigin.Begin);
            }
            catch (Exception)
            {
                throw new ImageUploadException("Couldn't read the contents of provided file.");
            }

            var uploadedImage = new AppImage { Id = Guid.NewGuid() };
            string extension = Path.GetExtension(file.FileName);
            var fileNameLarge = $"{uploadedImage.Id}_large{extension}";
            var fileNameSmall = $"{uploadedImage.Id}_small{extension}";

            string[] urls = await Task.WhenAll(
                _blobService.UploadAsync(largeImageStream, fileNameLarge, file.ContentType),
                _blobService.UploadAsync(smallImageStream, fileNameSmall, file.ContentType));

            uploadedImage.Large = urls[0];
            uploadedImage.Small = urls[1];
            await _unitOfWork.Images.AddAsync(uploadedImage);
        }

        await _unitOfWork.SaveAsync();
    }

    public async Task<IList<ImageBriefDto>> GetAvailableImagesAsync()
    {
        var images = await _unitOfWork.Images.GetAsync(
            predicate: i => i.GameId == null);
        return _mapper.Map<IList<ImageBriefDto>>(images);
    }

    private void ValidateFile(IFormFile file)
    {
        if (file.Length == 0)
        {
            throw new ImageUploadException("The provided file is empty.");
        }

        if (file.Length > _fileOptions.MaxImageSizeMb * 1024 * 1024)
        {
            throw new ImageUploadException($"The maximum allowed file size is {_fileOptions.MaxImageSizeMb} MB.");
        }

        if (!_fileOptions.ImageFormats.Contains(Path.GetExtension(file.FileName).ToUpperInvariant()))
        {
            throw new ImageUploadException($"Only {string.Join(", ", _fileOptions.ImageFormats)} formats are supported.");
        }
    }

    private static void ResizeImageIfBiggerThanMax(Image image, int maxWidth, int maxHeight)
    {
        if (image.Width <= maxWidth && image.Height <= maxHeight)
        {
            return;
        }

        double ratioX = (double)maxWidth / image.Width;
        double ratioY = (double)maxHeight / image.Height;
        double ratio = Math.Min(ratioX, ratioY);

        var width = (int)(image.Width * ratio);
        var height = (int)(image.Height * ratio);

        image.Mutate(x => x.Resize(width, height));
    }
}