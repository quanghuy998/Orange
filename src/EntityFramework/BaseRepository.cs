using DDD;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework
{
    public class BaseRepository<TAggregate> : IBaseRepository<TAggregate>
        where TAggregate : Aggregate
    {
        protected DbContext DbContext { get; }
        private readonly IMediator _mediator;

        public BaseRepository(DbContext dbContext, IMediator mediator)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mediator = mediator ?? throw new ArgumentNullException();
        }

        public Task CreateAsync(TAggregate aggreate, CancellationToken cancellationToken)
        {
            DbContext.Set<TAggregate>().AddAsync(aggreate, cancellationToken);
            return Task.CompletedTask;
        }

        public void Delete(TAggregate aggreate)
        {
            DbContext.Set<TAggregate>().Remove(aggreate);
        }

        public void Update(TAggregate aggreate)
        {
            DbContext.Set<TAggregate>().Update(aggreate);
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await _mediator.DispatchDomainEventsAsync(DbContext);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            var result = await DbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
