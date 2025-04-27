using Kafka.Messaging.Services.Abstractions;
using Microsoft.Extensions.Logging;
using SharedLibrary.ProjectModels;

namespace Kafka.Messaging.Services.Implementations
{
    public class TaskCreatedMessageHandler(ILogger<TaskCreatedMessageHandler> logger) : IMessageHandler<ItemModel>
    {
        public Task HandleAsync(ItemModel message, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Created new task {message.Title}");
            return Task.CompletedTask;
        }
    }
}
