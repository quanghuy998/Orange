using MediatR;

namespace DDD;
public interface IDomainEventConsumer<TNotification> : INotificationHandler<TNotification>
    where TNotification : INotification
{
}
