using RabbitMQ.Client;

namespace Infrasctructure.Queues
{
    public static class QueueConfiguration
    {
        public static void ConfigureQueues(IModel channel)
        {
            if (channel != null)
            {
                // Declaração da fila de pedidos
                channel.QueueDeclare(queue: "orders",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
            }
        }
    }
}
