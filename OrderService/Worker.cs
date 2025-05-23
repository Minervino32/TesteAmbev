using Application.Services;
using Domain.Dtos;
using Domain.Interfaces;
using Infrasctructure.Queues;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
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
        private readonly string _queueName = "orders";

        public Worker(IServiceScopeFactory scopeFactory, ILogger<Worker> logger, IOrderService orderService, IModel channel)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _orderService = orderService;
            _channel = channel;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                int retryCount = ea.BasicProperties.Headers?.ContainsKey("x-retry-count") == true ? Convert.ToInt32(ea.BasicProperties.Headers["x-retry-count"]) + 1 : 1;

                _logger.LogInformation($"Received Message: {message}");

                var stopwatch = Stopwatch.StartNew();

                try
                {
                    await ProcessMessage(message);
                    _logger.LogInformation($"Process concluded in {stopwatch.ElapsedMilliseconds} ms");
                    _channel.BasicAck(ea.DeliveryTag, false); // Confirma a mensagem processada
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError($"Error to Process Order ({stopwatch.ElapsedMilliseconds} ms): {ex.Message}");
                    if (retryCount >= 4)
                    {
                        _logger.LogWarning($"Message moved to DLQ after {retryCount} attempts!");
                        _channel.BasicNack(ea.DeliveryTag, false, false); // Envia para DLQ
                    }
                    else
                    {
                        var properties = _channel.CreateBasicProperties();
                        properties.Headers = new Dictionary<string, object> { { "x-retry-count", retryCount } };

                        _channel.BasicPublish(exchange: "", routingKey: "orders", basicProperties: properties, body: body);
                        _channel.BasicNack(ea.DeliveryTag, false, false);
                        _logger.LogInformation($"Message resent for retry {retryCount}!");
                    }
                }
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken); // Mantém Worker ativo
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
                    _logger.LogInformation($"Order Processed: {orderDto.Id}");
                }
                else
                {
                    _logger.LogWarning("Error in Desserealize of message!.");
                }
            }
        }
    }
}
