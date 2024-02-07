using System.Diagnostics.CodeAnalysis;

namespace GameStore.Shared.Exceptions;

[ExcludeFromCodeCoverage]
public class ImageUploadException : Exception
{
    public ImageUploadException(string message)
        : base(message)
    {
    }
}