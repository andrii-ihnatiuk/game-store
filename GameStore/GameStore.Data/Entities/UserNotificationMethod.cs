using GameStore.Data.Entities.Identity;

namespace GameStore.Data.Entities;

public class UserNotificationMethod
{
    public string UserId { get; set; }

    public Guid NotificationMethodId { get; set; }

    public NotificationMethod NotificationMethod { get; set; }

    public ApplicationUser User { get; set; }
}