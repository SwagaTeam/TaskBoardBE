using Confluent.Kafka;
using Kafka.Messaging.Services.Abstractions;
using Kafka.Messaging.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Kafka.Messaging.Services.Implementations
{
    public class KafkaConsumer<TMessage> : BackgroundService
    {
        private readonly string topic;
        private readonly IConsumer<string, TMessage> consumer;
        private readonly IServiceScopeFactory scopeFactory;

        public KafkaConsumer(IOptions<KafkaSettings> kafkaSettings, IServiceScopeFactory scopeFactory)
        {
            var config = new ConsumerConfig()
            {
                BootstrapServers = kafkaSettings.Value?.BootstrapServers,
                GroupId = kafkaSettings.Value?.GroupId
            };

            topic = kafkaSettings.Value?.Topic;

            consumer = new ConsumerBuilder<string, TMessage>(config)
                .SetValueDeserializer(new KafkaValueDeserealizer<TMessage>())
                .Build();

            this.scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => ConsumeAsync(stoppingToken), stoppingToken);
        }

        private async Task ConsumeAsync(CancellationToken stoppingToken)
        {
            Trace.TraceInformation("Subscribed to Kafka topic: " + topic);

            consumer.Subscribe(topic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(stoppingToken);
                    Console.WriteLine("GOT MESSAGE!");

                    Trace.TraceInformation("Message received: " + result?.Message?.Value?.ToString());

                    // Создаем scope, чтобы получить scoped сервис IMessageHandler<TMessage>
                    using var scope = scopeFactory.CreateScope();
                    var messageHandler = scope.ServiceProvider.GetRequiredService<IMessageHandler<TMessage>>();
                    
                    await messageHandler.HandleAsync(result.Message.Value, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            consumer.Close();
            return base.StopAsync(cancellationToken);
        }
    }
}
