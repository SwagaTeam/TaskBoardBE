using Confluent.Kafka;
using Kafka.Messaging.Services.Abstractions;
using Kafka.Messaging.Settings;
using Microsoft.Extensions.Options;

namespace Kafka.Messaging.Services.Implementations
{
    public class KafkaProducer<TMessage> : IKafkaProducer<TMessage>
    {
        private readonly IProducer<string, TMessage> producer;
        private readonly string topic;

        public KafkaProducer(IOptions<KafkaSettings> kafkaSettings)
        {
            var config = new ConsumerConfig()
            {
                BootstrapServers = kafkaSettings.Value?.BootstrapServers,
                GroupId = kafkaSettings.Value?.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            producer = new ProducerBuilder<string, TMessage>(config)
                .SetValueSerializer(new KafkaJsonSerializer<TMessage>())
                .Build();

            topic = kafkaSettings.Value.Topic;
        }
        public async Task ProduceAsync(TMessage message, CancellationToken cancellationToken)
        {
            await producer.ProduceAsync(
                topic,
                new Message<string, TMessage>
                {
                    Key = "1",
                    Value = message
                },
                cancellationToken);
            Console.WriteLine("✅ Message sent to Kafka.");
        }
        public void Dispose()
        {
            producer?.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
