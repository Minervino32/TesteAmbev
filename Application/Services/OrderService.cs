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
        private readonly IOrderNotificationService _orderNotificationService;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger, IOrderNotificationService orderNotificationService)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _orderNotificationService = orderNotificationService;
        }

        public async Task ProcessOrder(OrderDTO orderDto)
        {
            //verify if duplicated
            bool isProcessed = await _orderRepository.GetProcessed(orderDto.Id, orderDto.Requestor);
            if (isProcessed)
            {
                _logger.LogInformation("Order {id} already processed", orderDto.Id);
                throw new Exception($"Order {orderDto.Id} already processed");
            }

            decimal total = orderDto.Products.Sum(p => p.Price * p.Quantity);

            Order order = new()
            {
                ExternalId = orderDto.Id,
                Requestor = orderDto.Requestor,
                DateCreated = DateTime.UtcNow,
                Total = total,
                Status = OrderStatus.Processed,
                Products = orderDto.Products
            };

            //add to the database.
            Order newOrder = await _orderRepository.AddAsync(order);

            //send to queue
            await _orderNotificationService.PublishProcessedOrder(order);
        }
    }
}
