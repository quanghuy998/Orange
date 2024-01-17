using OrderService.Domain.Buyers;
using OrderService.Domain.Buyers.Interfaces;

namespace OrderService.Infrastructure.Buyers
{
    internal class BuyerRepository : BaseRepository<Buyer>, IBuyerRepository
    {
        public BuyerRepository(OrderDbContext context, IMediator mediator) : base(context, mediator)
        {
        }
    }
}
