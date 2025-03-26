using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

public class KafkaConsumerService : IHostedService
{
    private readonly string _bootstrapServers = "localhost:9092";
    private readonly string _topic = "user-notifications";

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = "notification-service-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        Task.Run(() => StartKafkaConsumer(config, cancellationToken), cancellationToken);
        return Task.CompletedTask;
    }

    private async Task StartKafkaConsumer(ConsumerConfig config, CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(_topic);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    Console.WriteLine($"Received message: {consumeResult.Message.Value}");

                    // Логика отправки уведомлений пользователю
                    await SendNotification(consumeResult.Message.Value);
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error: {e.Error.Reason}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Kafka consumer canceled");
        }
        finally
        {
            consumer.Close();
        }
    }

    private Task SendNotification(string message)
    {
        // Логика отправки уведомлений пользователю
        Console.WriteLine($"Sending notification: {message}");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Прекращение работы с Kafka
        return Task.CompletedTask;
    }
}