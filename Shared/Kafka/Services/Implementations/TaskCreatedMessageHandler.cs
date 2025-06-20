using Kafka.Messaging.Services.Abstractions;
using Microsoft.Extensions.Logging;
using SharedLibrary.Constants;
using SharedLibrary.Dapper.DapperRepositories.Abstractions;
using SharedLibrary.MailService;
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
            Console.WriteLine("handle");
            var item = message.UserItems;
            if (item == null || !item.Any())
            {
                logger.LogWarning("No user items found in the message.");
                return;
            }

            var userIds = item.Select(x => x.UserId).Distinct();
            var tasks = new List<Task>();
            var toList = new List<string>();
            foreach (var id in userIds)
            {
                logger.LogInformation($"Processing user with ID: {id}");
                var user = await userRepository.GetUserAsync(id);
                if (user is null)
                {
                    logger.LogWarning($"User with ID {id} not found.");
                    continue;
                }
                toList.Add(user.Email);
            }
            
            var subject = GetSubject(message.EventType);
            var body = $"Task update:\n{string.Join("\n", message.Message)}";
            tasks.Add(mailService.SendEmailAsync(subject, body, toList.ToArray()));
            
            await Task.WhenAll(tasks);
        }


        private static string GetSubject(TaskEventType type) => type switch
        {
            TaskEventType.Updated => "Задача изменена!",
            TaskEventType.AddedUser => "Добавлен новый пользователь в задачу!"
        };
    }
}