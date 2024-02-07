using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GameStore.Services.Interfaces.Util;
using GameStore.Shared.Options;
using Microsoft.Extensions.Options;

namespace GameStore.Services.Util;

public class BlobFileService : IBlobFileService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly AzureStorageOptions _storageOptions;

    public BlobFileService(BlobServiceClient blobServiceClient, IOptions<AzureStorageOptions> storageOptions)
    {
        _blobServiceClient = blobServiceClient;
        _storageOptions = storageOptions.Value;
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_storageOptions.ImagesContainer);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
        var blobClient = containerClient.GetBlobClient(fileName);

        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType },
        };

        await blobClient.UploadAsync(fileStream, uploadOptions);
        return blobClient.Uri.ToString();
    }
}