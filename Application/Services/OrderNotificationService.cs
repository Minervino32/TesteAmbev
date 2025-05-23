using Domain.Dtos;
using Domain.Entities;
using Domain.Extensions;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderNotificationService : IOrderNotificationService
    {
        private readonly ILogger<OrderNotificationService> _logger;
        private readonly IModel _channel; 

        public OrderNotificationService(ILogger<OrderNotificationService> logger, IModel channel)
        {
            _logger = logger;
            _channel = channel;
        }

        public async Task PublishProcessedOrder(Order order)
        {
            try
            {
                OrderProcessedViewModel orderProcessedViewModel = new()
                {
                    Id = order.ExternalId,
                    Requestor = order.Requestor,
                    DateCreated = DateTime.UtcNow,
                    Total = order.Total,
                    Products = order.Products,
                    Status = OrderStatus.Concluded,
                    StatusDescription = OrderStatus.Concluded.GetEnumDescription()
                };

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(orderProcessedViewModel));
                _channel.BasicPublish(
                    exchange: "",
                    routingKey: "orders_processed",
                    basicProperties: null,
                    body: body
                );

                _logger.LogInformation("Processed Order {id} sent to orders_processed queue", orderProcessedViewModel.Id);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
