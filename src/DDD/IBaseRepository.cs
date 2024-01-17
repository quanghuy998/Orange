namespace DDD;
public interface IBaseRepository<TAggregate>
    where TAggregate : Aggregate
{
    Task CreateAsync(TAggregate aggreate, CancellationToken cancellationToken);
    void Delete(TAggregate aggreate);
    void Update(TAggregate aggreate);
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken);
}
