using GameStore.Shared.DTOs.Notification;

namespace GameStore.Shared.DTOs.User;

public class ContactInfoDto
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public IList<NotificationMethodDto> NotificationMethods { get; set; }
}