using Kafka.Messaging.Services.Abstractions;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using SharedLibrary.Constants;
using SharedLibrary.Dapper.DapperRepositories.Abstractions;
using SharedLibrary.Models.KafkaModel;

namespace Kafka.Messaging.Services.Implementations
{
    public class TaskEventMessageHandler(
        ILogger<TaskEventMessageHandler> logger,
        IUserRepository userRepository,
        IEmailSender mailService
    ) : IMessageHandler<TaskEventMessage>
    {
        public async Task HandleAsync(TaskEventMessage message, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Received task event: {message.EventType} — {message.UserItems}");

            var item = message.UserItems;
            if (item == null || !item.Any())
            {
                logger.LogWarning("No user items found in the message.");
                return;
            }

            var userIds = item.Select(x => x.UserId).Distinct();
            var tasks = new List<Task>();

            foreach (var id in userIds)
            {
                logger.LogInformation($"Processing user with ID: {id}");
                var user = await userRepository.GetUserAsync(id);
                if (user is null)
                {
                    logger.LogWarning($"User with ID {id} not found.");
                    continue;
                }

                var subject = GetSubject(message.EventType);
                var body = $"Task update:\n{string.Join("\n", item)}";

                logger.LogInformation($"Sending email to {user.Email} with subject: {subject}");
                tasks.Add(mailService.SendEmailAsync(user.Email, subject, body));
            }

            await Task.WhenAll(tasks);
        }


        private static string GetSubject(TaskEventType type) => type switch
        {
            TaskEventType.Created => "Создана новая задача!",
            TaskEventType.Assigned => "Вам назначена задача",
            TaskEventType.StatusChanged => "Обновлен статус задачи",
            TaskEventType.CommentAdded => "Новый комментарий к задаче",
            TaskEventType.DocumentAttached => "К задаче прикреплен документ",
            _ => "Обновление по задаче"
        };
    }
}