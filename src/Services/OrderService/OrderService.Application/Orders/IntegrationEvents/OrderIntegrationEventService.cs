using EventBus;
using EventBus.Abstractions;
using EventStore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Infrastructure;
using System.Data.Common;

namespace OrderService.Application.Orders.IntegrationEvents;
public class OrderIntegrationEventService : IOrderIntegrationEventService
{
    private readonly IEventBus _eventBus;
    private readonly IIntegrationEventLogService _eventLogService;
    private readonly OrderDbContext _orderDbContext;
    private readonly ILogger<OrderIntegrationEventService> _logger;

    public OrderIntegrationEventService(IEventBus eventBus,
                                        IIntegrationEventLogService eventLogService,
                                        OrderDbContext orderDbContext,
                                        ILogger<OrderIntegrationEventService> logger)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _eventLogService = eventLogService ?? throw new ArgumentNullException(nameof(eventLogService));
        _orderDbContext = orderDbContext ?? throw new ArgumentNullException(nameof(orderDbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
    {
        var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

        foreach (var pendingLogEvent in pendingLogEvents)
        {
            _logger.LogInformation($"Publising integration event: {pendingLogEvent.EventId} - {pendingLogEvent.IntegrationEvent}");

            try
            {
                await _eventLogService.MarkEventAsInProgressAsync(pendingLogEvent.EventId);
                _eventBus.Publish(pendingLogEvent.IntegrationEvent);
                await _eventLogService.MarkEventAsPublishedAsync(pendingLogEvent.EventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error publishing integration event: {pendingLogEvent.EventId}");
                await _eventLogService.MarkEventAsFailedAsync(pendingLogEvent.EventId);
            }
        }
    }

    public async Task AddAndSaveEventAsync(IntegrationEvent @event)
    {
        _logger.LogInformation($"Enqueuing integration event {@event.Id} to repository {@event}");
        await _eventLogService.SaveEventAsync(@event, _orderDbContext.GetCurrentTransaction());
    }
}
