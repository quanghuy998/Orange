using EventBus;
using EventStore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace EventStore.Services
{
    public class IntegrationEventLogService : IIntegrationEventLogService
    {
        private readonly IntegrationEventLogContext _dbContext;
        private readonly List<Type> _eventTypes;
        private volatile bool _disposedValue;

        public IntegrationEventLogService(IntegrationEventLogContext dbContext, Assembly assembly)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _eventTypes = assembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("IntegrationEvent"))
                .ToList();
        }

        public async Task<IEnumerable<IntegrationEventLog>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId)
        {
            var tid = transactionId.ToString();

            var result = await _dbContext.IntegrationEventLogs
                .Where(e => e.TransactionId == tid && e.State == EventState.NotPublished)
                .ToListAsync();

            if (result.Any())
            {
                return result
                    .OrderBy(o => o.CreatedTime)
                    .Select(e =>
                        e.DeserializeJsonContent(
                            _eventTypes.Find(t => t.Name == e.EventTypeShortName)));
            }

            return new List<IntegrationEventLog>();
        }

        public Task MarkEventAsFailedAsync(Guid eventId)
        {
            return UpdateEventsStatus(eventId, EventState.Failed);
        }

        public Task MarkEventAsInProgressAsync(Guid eventId)
        {
            return UpdateEventsStatus(eventId, EventState.InProgress);
        }

        public Task MarkEventAsPublishedAsync(Guid eventId)
        {
            return UpdateEventsStatus(eventId, EventState.Published);
        }

        public Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var eventLog = new IntegrationEventLog(@event, transaction.TransactionId);

            //_dbContext.Database.UseTransaction(transaction.GetDbTransaction());
            _dbContext.IntegrationEventLogs.Add(eventLog);

            return _dbContext.SaveChangesAsync();
        }

        private Task UpdateEventsStatus(Guid guid, EventState state)
        {
            var eventLog = _dbContext.IntegrationEventLogs.Single(ie => ie.EventId == guid);
            eventLog.State = state;

            if (state == EventState.InProgress)
                eventLog.TimesSent++;

            _dbContext.IntegrationEventLogs.Update(eventLog);

            return _dbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }


                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
