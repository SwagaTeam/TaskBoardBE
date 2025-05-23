
using Confluent.Kafka;
using Kafka.Messaging.Services.Abstractions;
using Kafka.Messaging.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Kafka.Messaging.Services.Implementations
{
    public class KafkaConsumer<TMessage> : BackgroundService
    {
        private readonly string topic;
        private readonly IConsumer<string, TMessage> consumer;
        private readonly IMessageHandler<TMessage> messageHandler;


        public KafkaConsumer(IOptions<KafkaSettings> kafkaSettings, IMessageHandler<TMessage> messageHandler)
        {
            var config = new ConsumerConfig()
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers,
                GroupId = kafkaSettings.Value.GroupId
            };

            topic = kafkaSettings.Value.Topic;

            consumer = new ConsumerBuilder<string, TMessage>(config)
                .SetValueDeserializer(new KafkaValueDeserealizer<TMessage>())
                .Build();

            this.messageHandler = messageHandler;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => ConsumeAsync(stoppingToken), stoppingToken);
        }

        private async Task? ConsumeAsync(CancellationToken stoppingToken)
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
