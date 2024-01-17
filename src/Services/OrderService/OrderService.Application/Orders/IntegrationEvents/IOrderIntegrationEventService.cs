using EventBus;

namespace OrderService.Application.Orders.IntegrationEvents;
public interface IOrderIntegrationEventService
{
    Task AddAndSaveEventAsync(IntegrationEvent @event);
    Task PublishEventsThroughEventBusAsync(Guid transactionId);
}