namespace Kafka.Messaging.Services.Abstractions
{
    public interface IKafkaProducer<in TMessage> : IDisposable
    {
        Task ProduceAsync(TMessage message, CancellationToken cancellationToken);
    }
}
