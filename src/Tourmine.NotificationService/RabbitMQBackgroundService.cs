using Tourmine.NotificationService.Consumers;

public class RabbitMqBackgroundService : BackgroundService
{
    private readonly RabbitMqConsumer _consumer;

    public RabbitMqBackgroundService(RabbitMqConsumer consumer)
    {
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consumer.StartListeningAsync();
    }
}