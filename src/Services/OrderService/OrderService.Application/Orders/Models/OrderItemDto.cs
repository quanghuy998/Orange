﻿namespace OrderService.Application.Orders.Models
{
    public class OrderItemDto
    {
        public int ProductId { get; init; }
        public string ProductName { get; init; }
        public decimal UnitPrice { get; init; }
        public decimal Discount { get; init; }
        public int Units { get; init; }
        public string PictureUrl { get; init; }
    }
}
