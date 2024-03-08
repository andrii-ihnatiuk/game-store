namespace GameStore.Shared.Options;

public class FileUploadOptions
{
    public int MaxImageSizeMb { get; init; }

    public IReadOnlyList<string> ImageFormats { get; init; }
}