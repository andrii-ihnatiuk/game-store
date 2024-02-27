namespace GameStore.Shared.DTOs.Publisher;

public class PublisherUpdateDto
{
    public string Culture { get; set; }

    public PublisherUpdateInnerDto Publisher { get; set; }
}