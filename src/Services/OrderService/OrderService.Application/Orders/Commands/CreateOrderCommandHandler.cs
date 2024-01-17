using CQRS.Commands;
using OrderService.Domain.Orders;
using OrderService.Domain.Orders.Entities;
using OrderService.Domain.Orders.Interfaces;
using OrderService.Application.Orders.IntegrationEvents;
using OrderService.Application.Orders.IntegrationEvents.Events;

namespace OrderService.Application.Orders.Commands;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private IOrderIntegrationEventService _integrationEventService;
    public CreateOrderCommandHandler(IOrderRepository orderRepository,
                                     IOrderIntegrationEventService integrationEventService)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
    }

    public async Task<CommandResult<bool>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var address = new Address(command.Street,
                                  command.City,
                                  command.State,
                                  command.Country,
                                  command.ZipCode);

        var order = new Order(command.UserId,
                              command.UserName,
                              address,
                              command.CardTypeId,
                              command.CardNumber,
                              command.CardSecurityNumber,
                              command.CardHolderName,
                              command.CardExpiration);

        foreach (var item in command.OrderItems)
            order.AddOrderItem(item.ProductId,
                               item.ProductName,
                               item.UnitPrice,
                               item.Discount,
                               item.PictureUrl,
                               item.Units);

        await _orderRepository.CreateAsync(order, cancellationToken);
        await _orderRepository.SaveEntitiesAsync(cancellationToken);

        await _integrationEventService.AddAndSaveEventAsync(new OrderStartedIntegrationEvent(command.UserId));
        return CommandResult<bool>.Success(true);
    }
}
