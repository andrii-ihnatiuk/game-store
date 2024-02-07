namespace GameStore.Services.Interfaces.Util;

public interface IBlobFileService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
}