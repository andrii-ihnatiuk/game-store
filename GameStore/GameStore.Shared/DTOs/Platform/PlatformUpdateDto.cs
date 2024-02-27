namespace GameStore.Shared.DTOs.Platform;

public class PlatformUpdateDto
{
    public string Culture { get; set; }

    public PlatformUpdateInnerDto Platform { get; set; }
}