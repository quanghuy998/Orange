using EventBus;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace EventStore.Models;
public class IntegrationEventLog
{
    private static readonly JsonSerializerOptions s_indentedOptions = new() { WriteIndented = true };
    private static readonly JsonSerializerOptions s_caseInsensitiveOptions = new() { PropertyNameCaseInsensitive = true };

    public Guid EventId { get; private set; }
    public string EventTypeName { get; private set; }
    [NotMapped]
    public string EventTypeShortName => EventTypeName.Split('.').Last();
    [NotMapped]
    public IntegrationEvent IntegrationEvent { get; private set; }
    public EventState State { get; set; }
    public int TimesSent { get; set; }
    public DateTime CreatedTime { get; private set; }
    public string Content { get; private set; }
    public string TransactionId { get; private set; }

    private IntegrationEventLog() { }
    public IntegrationEventLog(IntegrationEvent @event, Guid transactionId)
    {
        EventId = @event.Id;
        CreatedTime = @event.CreatedTime;
        EventTypeName = @event.GetType().FullName;
        Content = JsonSerializer.Serialize(@event, @event.GetType(), s_indentedOptions);
        State = EventState.NotPublished;
        TimesSent = 0;
        TransactionId = transactionId.ToString();
    }

    public IntegrationEventLog DeserializeJsonContent(Type type)
    {
        IntegrationEvent = JsonSerializer.Deserialize(Content, type, s_caseInsensitiveOptions) as IntegrationEvent;
        return this;
    }
}
