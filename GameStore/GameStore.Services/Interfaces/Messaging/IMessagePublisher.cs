using GameStore.Services.Models;

namespace GameStore.Services.Interfaces.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<T>(TopicMessage<T> message)
        where T : class;
}