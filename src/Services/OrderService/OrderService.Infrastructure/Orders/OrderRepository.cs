using OrderService.Domain.Orders.Interfaces;
using OrderService.Domain.Orders;

namespace OrderService.Infrastructure.Orders;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(OrderDbContext context, IMediator mediator) : base(context, mediator)
    {
    }
}
