using DDD;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Orders.Events;

namespace OrderService.Application.Orders.DomainEventConsumers
{
    public class OrderCreatedDomainEventConsumer : IDomainEventConsumer<OrderCreatedDomainEvent>
    {
        private readonly ILogger<OrderCreatedDomainEventConsumer> _logger;

        public OrderCreatedDomainEventConsumer(ILogger<OrderCreatedDomainEventConsumer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling domain event: ({@IntegrationEvent})", notification);
            return Task.CompletedTask;
        }
    }
}
