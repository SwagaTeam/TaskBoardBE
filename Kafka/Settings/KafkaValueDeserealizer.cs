using Confluent.Kafka;
using System.Text.Json;

namespace Kafka.Messaging.Settings
{
    public class KafkaValueDeserealizer<TMessage> : IDeserializer<TMessage>
    {
        public TMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return JsonSerializer.Deserialize<TMessage>(data)!;
        }
    }
}
