using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using Tourmine.NotificationService.Models;
using Tourmine.NotificationService.Services;

namespace Tourmine.NotificationService.Consumers
{
    public class RabbitMqConsumer 
    {
        private readonly ConnectionFactory _factory;
        private readonly HttpClient _httpClient;

        public RabbitMqConsumer(HttpClient httpClient)
        {
            _factory = new ConnectionFactory() { HostName = "localhost"};
            _httpClient = httpClient;
        }

        public async Task StartListeningAsync()
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync("tournament.notifications", ExchangeType.Direct);

            string queueSubscription = "tournament.subscription";
            await channel.QueueDeclareAsync(queueSubscription, durable: false, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueBindAsync(queueSubscription, "tournament.notifications", "subscription");

            string queueCreation = "tournament.creation";
            await channel.QueueDeclareAsync(queueCreation, durable: false, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueBindAsync(queueCreation, "tournament.notifications", "creation");

            Console.WriteLine("Waiting for messages...");

            var consumerSubscription = new AsyncEventingBasicConsumer(channel);
            consumerSubscription.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await ProcessMessageAsync(message, "Subscription");

                await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            };

            var consumerCreation = new AsyncEventingBasicConsumer(channel);
            consumerCreation.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await ProcessMessageAsync(message, "TournamentCreated");

                await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            };

            // Consumindo as filas SEPARADAMENTE
            await channel.BasicConsumeAsync(queueSubscription, autoAck: false, consumerSubscription);
            await channel.BasicConsumeAsync(queueCreation, autoAck: false, consumerCreation);

            // Isso garante que o programa vai esperar indefinidamente pelas mensagens
            await Task.Delay(Timeout.Infinite);
        }

        private async Task ProcessMessageAsync(string message, string type)
        {
            var notification = JsonSerializer.Deserialize<NotificationSubscription>(message);

            if (type == "Subscription")
            {
                Console.WriteLine($"New Subscription: Tournament ID: {notification.TournamentId}, User ID: {notification.UserId}");

                var response = await _httpClient.GetAsync($"{Settings.TournamentBasePath}{notification.TournamentId}");
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Usa o camelCase para deserializar
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // Ignora propriedades nulas
                    };

                    var tournament = JsonSerializer.Deserialize<Tournament>(responseContent, options);

                    var responseOrganizer = await _httpClient.GetAsync($"{Settings.UserBasePath}{tournament.UserId}");
                    var responseContentOrganizer = await responseOrganizer.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response content organizer: {responseContent}");

                    if (responseOrganizer.IsSuccessStatusCode)
                    {
                        var organizer = JsonSerializer.Deserialize<User>(responseContentOrganizer, options);
                        var organizerEmail = organizer.Email;

                        await EmailService.SendEmailAsync(organizerEmail, "New Tournament Subscription", $"A new user has subscribed to your tournament with ID: {notification.UserId}");
                    }
                    else
                    {
                        Console.WriteLine("Erro ao obter organizador.");
                    }
                }
                else
                {
                    Console.WriteLine("Erro ao obter usuário.");
                }
            }
            else if (type == "TournamentCreated")
            {
                Console.WriteLine($"New Tournament Created: Tournament ID: {notification.TournamentId}");
            }

            await Task.CompletedTask;
        }
    }
}
