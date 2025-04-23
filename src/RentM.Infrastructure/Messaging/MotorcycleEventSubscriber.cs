using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RentM.Domain.Models;
using RentM.Infrastructure.Interfaces;
using System.Text;
using System.Text.Json;

namespace RentM.Infrastructure.Messaging
{
    public class MotorcycleEventSubscriber
    {
        private readonly string _hostName;
        private readonly string _queueName;
        private readonly IServiceProvider _serviceProvider;

        public MotorcycleEventSubscriber(string hostName, string queueName, IServiceProvider serviceProvider)
        {
            _hostName = hostName;
            _queueName = queueName;
            _serviceProvider = serviceProvider;
        }

        public void StartListening()
        {
            
            var factory = new ConnectionFactory() { HostName = _hostName };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var motorcycleEvent = new MotorcycleEvent()
                {
                    Id = Guid.NewGuid(),
                    EventType = "MotorcycleRegistered",
                    Payload = message,
                    Timestamp = DateTime.UtcNow
                };

                if (motorcycleEvent != null)
                {
                    await SaveData(motorcycleEvent);
                }

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: _queueName,
                                 autoAck: false,
                                 consumer: consumer);
        }

        public async Task SaveData(MotorcycleEvent motorcycleEvent)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IMotorcycleEventRepository>();
                await repository.SaveEventAsync(motorcycleEvent);

            }

        }
    }
}
