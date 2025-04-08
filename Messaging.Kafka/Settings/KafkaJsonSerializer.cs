using Confluent.Kafka;
using System.Text.Json;

namespace Messaging.Kafka.Settings
{
    public class KafkaJsonSerializer<TMessage> : ISerializer<TMessage>
    {
        public byte[] Serialize(TMessage data, SerializationContext context)
        {
            return JsonSerializer.SerializeToUtf8Bytes(data);
        }
    }
}
