namespace Messaging.Kafka.Services.Abstractions
{
    public interface IMessageHandler<TMessage>
    {
        Task HandleAsync(TMessage message, CancellationToken cancellationToken);
    }
}
