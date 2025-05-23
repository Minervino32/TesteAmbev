using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task ProcessOrder(OrderDTO orderDto)
        {
            bool isProcessed = await _orderRepository.GetProcessed(orderDto.Id);
            if (isProcessed)
            {
                throw new Exception("Order already processed");
            }

            orderDto.Status = OrderStatus.Processed;
            orderDto.Total = orderDto.Products.Sum(p => p.Price * p.Quantity);

            Order order = new()
            {
                ExternalId = orderDto.Id,
                DateCreated = orderDto.DateCreated,
                Total = orderDto.Total,
                Status = orderDto.Status,
                Products = orderDto.Products // Ensure Products is properly initialized  
            };

            Order newOrder = await _orderRepository.AddAsync(order);
        }
    }
}
