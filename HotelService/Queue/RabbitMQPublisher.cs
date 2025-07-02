using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace HotelService.Queue
{
    public class RabbitMQPublisher
    {
        private readonly string _hostname = "localhost";
        private readonly string _queueName = "hotel_reservations";
        private IConnection _connection;

        public RabbitMQPublisher()
        {
            var factory = new ConnectionFactory() { HostName = _hostname };
            _connection = factory.CreateConnection();
        }

        public void Publish<T>(T message)
        {
            try
            {
                using var channel = _connection.CreateModel();
                channel.QueueDeclare(queue: _queueName,
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                                    routingKey: _queueName,
                                    basicProperties: properties,
                                    body: body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RabbitMQ ERROR] {ex.Message}");
            }
        }
    }
}
