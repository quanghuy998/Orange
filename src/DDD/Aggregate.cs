using MediatR;

namespace DDD;
public class Aggregate : Entity
{
    int? _requestedHashCode;
    private List<INotification> _domainEvents;
    public List<INotification> DomainEvents => _domainEvents;

    public void AddDomainEvent(INotification domainEvent)
    {
        _domainEvents = _domainEvents ?? new List<INotification>();
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(INotification domainEvent)
    {
        if (_domainEvents is null) return;
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }

}
