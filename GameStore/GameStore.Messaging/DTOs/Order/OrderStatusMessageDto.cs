namespace GameStore.Messaging.DTOs.Order;

public class OrderStatusMessageDto
{
    public string OrderId { get; set; }

    public string Status { get; set; }

    public string RecipientName { get; set; }

    public string RecipientEmail { get; set; }

    public string RecipientPhoneNumber { get; set; }

    public string RecipientDeviceToken { get; set; }
}