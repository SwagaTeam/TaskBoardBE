namespace Messaging.Kafka.Services.Abstractions
{
    public interface IKafkaProducer<in TMessage> : IDisposable
    {
        Task ProduceAsync(TMessage message, CancellationToken cancellationToken);
    }
}
