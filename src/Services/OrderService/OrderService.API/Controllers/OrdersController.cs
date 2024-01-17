using CQRS.Commands;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Models;
using OrderService.Application.Orders.Commands;
using OrderService.Application.Orders.IntegrationEvents;

namespace OrderService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ICommandBus _commandBus;

        public OrdersController(ICommandBus commandBus)
        {
            _commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderRequest request)
        {
            var command = new CreateOrderCommand(request.OrderItems.ToList(),
                                                 request.UserId,
                                                 request.UserName,
                                                 request.City,
                                                 request.Street,
                                                 request.State,
                                                 request.Country,
                                                 request.ZipCode,
                                                 request.CardNumber,
                                                 request.CardHolderName,
                                                 request.CardExpiration,
                                                 request.CardSecurityNumber,
                                                 request.CardTypeId);

            var result = await _commandBus.SendAsync(command);

            return Ok(result);
        }
    }
}
