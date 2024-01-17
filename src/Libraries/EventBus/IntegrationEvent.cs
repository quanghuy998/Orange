using System.Text.Json.Serialization;

namespace EventBus;

public record IntegrationEvent
{
    [JsonInclude]
    public Guid Id { get; private init; }

    [JsonInclude]
    public DateTime CreatedTime { get; private init; }

    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreatedTime = DateTime.UtcNow;
    }

    [JsonConstructor]
    public IntegrationEvent(Guid id, DateTime createDate)
    {
        Id = id;
        CreatedTime = createDate;
    }
}
