namespace Kafka.Messaging.Services.Abstractions
{
    public interface IMessageHandler<TMessage>
    {
        Task HandleAsync(TMessage message, CancellationToken cancellationToken);
    }
}
