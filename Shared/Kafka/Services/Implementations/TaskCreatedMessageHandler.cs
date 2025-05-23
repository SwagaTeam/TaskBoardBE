using Kafka.Messaging.Services.Abstractions;
using Microsoft.Extensions.Logging;
using SharedLibrary.Dapper.DapperRepositories;
using SharedLibrary.MailService;
using SharedLibrary.ProjectModels;

namespace Kafka.Messaging.Services.Implementations
{
    public class TaskCreatedMessageHandler(ILogger<TaskCreatedMessageHandler> logger) : IMessageHandler<ItemModel>
    {
        public async Task HandleAsync(ItemModel message, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Created new task {message.Title}");
            Console.WriteLine($"HANDLE: Created task {message.Title}");

            var userIds = message.UserItems.Select(x => x.Id);

            foreach (var id in userIds)
            {
                var user = await UserRepository.GetUser(id);

                if (user is not null)
                {
                    await MailService.SendEmailAsync(user.Email, "Новая задача!", $"{message.Title} - {message.Description}");
                }
            }
                
            return;
        }
    }
}
