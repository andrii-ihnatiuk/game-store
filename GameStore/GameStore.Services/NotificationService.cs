using AutoMapper;
using GameStore.Data.Interfaces;
using GameStore.Messaging.Constants;
using GameStore.Messaging.DTOs.Order;
using GameStore.Services.Interfaces;
using GameStore.Services.Interfaces.Messaging;
using GameStore.Services.Models;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Notification;
using GameStore.Shared.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GameStore.Services;

public class NotificationService : INotificationService
{
    private const char MethodsDelimiter = ',';

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMessagePublisher _messagePublisher;
    private readonly AzureServiceBusOptions _serviceBusOptions;

    public NotificationService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessagePublisher messagePublisher,
        IOptions<AzureServiceBusOptions> serviceBusOptions)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _messagePublisher = messagePublisher;
        _serviceBusOptions = serviceBusOptions.Value;
    }

    public async Task<IList<NotificationMethodDto>> GetNotificationMethodsAsync()
    {
        var methods = await _unitOfWork.NotificationMethods.GetAsync();
        return _mapper.Map<IList<NotificationMethodDto>>(methods);
    }

    public async Task NotifyOrderStatusChangedAsync(string orderId, OrderStatus status)
    {
        var order = await _unitOfWork.Orders.GetOneAsync(
            predicate: o => o.Id == Guid.Parse(orderId),
            include: q => q.Include(o => o.Customer)
                .ThenInclude(c => c.NotificationMethods)
                .ThenInclude(x => x.NotificationMethod));

        if (order.Customer.NotificationMethods.Count == 0)
        {
            return;
        }

        string notificationMethods = string.Join(
            MethodsDelimiter, order.Customer.NotificationMethods.Select(x => x.NotificationMethod.NormalizedName));
        var messageBody = new OrderStatusMessageDto
        {
            Status = status.ToString(),
            OrderId = orderId,
            RecipientName = order.Customer.UserName,
            RecipientEmail = order.Customer.Email,
            RecipientPhoneNumber = order.Customer.PhoneNumber,
            RecipientDeviceToken = Guid.Empty.ToString(),
        };

        var message = new TopicMessage<OrderStatusMessageDto>
        {
            TopicName = _serviceBusOptions.NotificationsTopic,
            Properties = new Dictionary<string, object>
            {
                { MessageProperties.NotifyVia, notificationMethods },
                { MessageProperties.MessageType, nameof(OrderStatusMessageDto) },
            },
            Value = messageBody,
        };

        await _messagePublisher.PublishAsync(message);
    }
}