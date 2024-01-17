using EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using OrderService.Application.Orders.IntegrationEvents.Events;

namespace OrderService.Application.Orders.IntegrationEvents.EventHandlers
{
    public class OrderStartedIntegrationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
    {
        private readonly ILogger<OrderStartedIntegrationEventHandler> _logger;

        public OrderStartedIntegrationEventHandler(ILogger<OrderStartedIntegrationEventHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        public Task Handle(OrderStartedIntegrationEvent @event)
        {
            _logger.LogInformation("Handling integration event: ({@IntegrationEvent})", @event);
            return Task.CompletedTask;
        }
    }
}
