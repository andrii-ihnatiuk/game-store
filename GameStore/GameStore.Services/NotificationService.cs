using AutoMapper;
using GameStore.Data.Entities.Identity;
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
        foreach (var user in await GetCustomerAndAdminsToNotifyAsync(Guid.Parse(orderId)))
        {
            string notificationMethods = string.Join(
                MethodsDelimiter, user.NotificationMethods.Select(x => x.NotificationMethod.NormalizedName));
            var messageBody = new OrderStatusMessageDto
            {
                Status = status.ToString(),
                OrderId = orderId,
                RecipientName = user.UserName,
                RecipientEmail = user.Email,
                RecipientPhoneNumber = user.PhoneNumber,
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

    private async Task<IList<ApplicationUser>> GetCustomerAndAdminsToNotifyAsync(Guid orderId)
    {
        var customer = (await _unitOfWork.Orders.GetOneAsync(
            predicate: o => o.Id == orderId,
            include: q => q.Include(o => o.Customer)
                .ThenInclude(c => c.NotificationMethods)
                .ThenInclude(x => x.NotificationMethod)))
            .Customer;
        var adminUsers = (await _unitOfWork.UsersRoles.GetAsync(
            predicate: x => x.Role.NormalizedName.Equals(Roles.Admin.ToUpperInvariant()),
            include: q => q.Include(x => x.User)
                .ThenInclude(u => u.NotificationMethods)
                .ThenInclude(nm => nm.NotificationMethod)))
            .Select(x => x.User);

        var usersToNotify = adminUsers.Append(customer)
            .Where(u => u.NotificationMethods.Count > 0)
            .GroupBy(u => u.Id)
            .Select(x => x.First());
        return usersToNotify.ToList();
    }
}