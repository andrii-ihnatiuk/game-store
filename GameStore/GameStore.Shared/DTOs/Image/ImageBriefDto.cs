namespace GameStore.Shared.DTOs.Image;

public class ImageBriefDto
{
    public Guid Id { get; set; }

    public string Large { get; set; }

    public string? Small { get; set; }

    public bool IsCover { get; set; }
}