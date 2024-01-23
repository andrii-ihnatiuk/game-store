using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Notification;

namespace GameStore.Services.Interfaces;

public interface INotificationService
{
    Task<IList<NotificationMethodDto>> GetNotificationMethodsAsync();

    Task NotifyOrderStatusChangedAsync(string orderId, OrderStatus status);
}