using Messaging.Kafka.Services.Abstractions;
using SharedLibrary.ProjectModels;

namespace Messaging.Kafka.Services.Implementations
{
    public class TaskCreatedMessageHandler(ILogger<TaskCreatedMessageHandler> logger) : IMessageHandler<TaskModel>
    {
        public Task HandleAsync(TaskModel message, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Created new task {message.Title}");
            return Task.CompletedTask;
        }
    }
}
