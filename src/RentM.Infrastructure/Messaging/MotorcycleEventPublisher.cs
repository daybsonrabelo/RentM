using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using RentM.Domain.Models;

namespace RentM.Infrastructure.Messaging
{
    public class MotorcycleEventPublisher
    {
        private readonly string _hostName;
        private readonly string _queueName;

        public MotorcycleEventPublisher(string hostName, string queueName)
        {
            _hostName = hostName;
            _queueName = queueName;
        }

        public async Task PublishMotorcycleRegisteredEventAsync(Motorcycle motorcycle)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var message = JsonSerializer.Serialize(new
                {
                    motorcycle.Id,
                    motorcycle.Year,
                    motorcycle.Model,
                    motorcycle.LicensePlate
                });

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: _queueName,
                                     basicProperties: null,
                                     body: body);
            }

            await Task.CompletedTask;
        }
    }
}
