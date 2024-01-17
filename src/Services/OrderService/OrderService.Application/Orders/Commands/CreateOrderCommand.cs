using CQRS.Commands;
using OrderService.Application.Orders.Models;

namespace OrderService.Application.Orders.Commands;
public class CreateOrderCommand : ICommand<bool>
{
    private readonly List<OrderItemDto> _orderItems;

    public string UserId { get; init; }
    public string UserName { get; init; }
    public string City { get; init; }
    public string Street { get; init; }
    public string State { get; init; }
    public string Country { get; init; }
    public string ZipCode { get; init; }
    public string CardNumber { get; init; }
    public string CardHolderName { get; init; }
    public DateTime CardExpiration { get; init; }
    public string CardSecurityNumber { get; init; }
    public int CardTypeId { get; init; }

    public IEnumerable<OrderItemDto> OrderItems => _orderItems;

    protected CreateOrderCommand()
    {
        _orderItems = new List<OrderItemDto>();
    }

    //public CreateOrderCommand(List<BasketItem> basketItems, string userId, string userName
    //    , string city, string street, string state, string country, string zipcode
    //    , string cardNumber, string cardHolderName, DateTime cardExpiration
    //    , string cardSercurityNumber, int cardTypeId) : this()
    //{
    //    _orderItems = basketItems.ToOrderItemsDto().ToList();
    //    UserId = userId;
    //    UserName = userName;
    //    City = city;
    //    Street = street;
    //    State = state;
    //    Country = country;
    //    ZipCode = zipcode;
    //    CardNumber = cardNumber;
    //    CardHolderName = cardHolderName;
    //    CardExpiration = cardExpiration;
    //    CardHolderName = cardHolderName;
    //    CardExpiration = cardExpiration;
    //    CardSecurityNumber = cardSercurityNumber;
    //    CardTypeId = cardTypeId;
    //}

    public CreateOrderCommand(List<OrderItemDto> orderItemDtos,
                              string userId,
                              string userName,
                              string city,
                              string street,
                              string state,
                              string country,
                              string zipcode,
                              string cardNumber,
                              string cardHolderName,
                              DateTime cardExpiration,
                              string cardSercurityNumber,
                              int cardTypeId) : this()
    {
        _orderItems = orderItemDtos;
        UserId = userId;
        UserName = userName;
        City = city;
        Street = street;
        State = state;
        Country = country;
        ZipCode = zipcode;
        CardNumber = cardNumber;
        CardHolderName = cardHolderName;
        CardExpiration = cardExpiration;
        CardHolderName = cardHolderName;
        CardExpiration = cardExpiration;
        CardSecurityNumber = cardSercurityNumber;
        CardTypeId = cardTypeId;
    }
}
