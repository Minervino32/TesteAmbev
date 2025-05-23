using Domain.Dtos;
using Domain.Interfaces;
using Infrasctructure.Queues;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace OrderService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IOrderService _orderService;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IModel _channel;

        public Worker(IServiceScopeFactory scopeFactory, ILogger<Worker> logger, IOrderService orderService, IModel channel)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _orderService = orderService;
            _channel = channel;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.Received += async (sender, eventArgs) =>
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    _logger.LogInformation("Received message: {message}", message);
                    await ProcessMessage(message);
                };

                _channel.BasicConsume(queue: "orders", autoAck: true, consumer: consumer);

                await Task.CompletedTask;

            }
            catch (Exception ex)
            {
                _logger.LogError("Error to try process message: {ex}", ex.Message);
                throw;
            }
        }

        private async Task ProcessMessage(string message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                OrderDTO orderDto = JsonSerializer.Deserialize<OrderDTO>(message);
                if (orderDto != null)
                {
                    await _orderService.ProcessOrder(orderDto);
                }
            }
        }
    }
}
