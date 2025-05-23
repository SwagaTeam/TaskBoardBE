using Confluent.Kafka;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Kafka.Messaging.Settings
{
    public class KafkaValueDeserealizer<TMessage> : IDeserializer<TMessage>
    {
        public TMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            try
            {
                var json = Encoding.UTF8.GetString(data);
                return JsonSerializer.Deserialize<TMessage>(json)!;
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Kafka deserialization error: {ex.Message}");
                throw;
            }
        }

    }
}
