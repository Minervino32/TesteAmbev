using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task ProcessOrder(OrderDTO orderDto)
        {
            bool isProcessed = await _orderRepository.GetProcessed(orderDto.Id);
            if (isProcessed)
            {
                _logger.LogInformation("Order {id} already processed", orderDto.Id);
                throw new Exception($"Order {orderDto.Id} already processed");
            }

            decimal total = orderDto.Products.Sum(p => p.Price * p.Quantity);

            Order order = new()
            {
                ExternalId = orderDto.Id,
                DateCreated = DateTime.UtcNow,
                Total = total,
                Status = OrderStatus.Processed,
                Products = orderDto.Products
            };

            Order newOrder = await _orderRepository.AddAsync(order);
        }
    }
}
