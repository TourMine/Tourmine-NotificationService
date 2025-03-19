using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Tourmine.NotificationService.Models;

namespace Tourmine.NotificationService.Consumers
{
    public class RabbitMqConsumer 
    {
        private readonly ConnectionFactory _factory;

        public RabbitMqConsumer()
        {
            _factory = new ConnectionFactory() { HostName = "localhost"};
        }

        public async Task StartListeningAsync()
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync("tournament.notifications", ExchangeType.Fanout);

            var queueName = await channel.QueueDeclareAsync("", exclusive: true);
            await channel.QueueBindAsync(queueName, "tournament.notifications", "");

            Console.WriteLine("Listening for messages...");

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                await ProcessMessageAsync(message);
            };

            await channel.BasicConsumeAsync(queueName, autoAck: true, consumer);

            await Task.Delay(-1);
        }

        private async Task ProcessMessageAsync(string message)
        {
            var notification = JsonSerializer.Deserialize<NotificationSubscription>(message);
            Console.WriteLine($"Received notification: Tournament ID: {notification.TournamentId}, User ID: {notification.UserId}");

            await Task.CompletedTask;
        }
    }
}
