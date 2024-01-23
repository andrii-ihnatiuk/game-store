namespace GameStore.Services.Models;

public class TopicMessage<T>
{
    public string TopicName { get; set; }

    public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

    public T Value { get; set; }
}