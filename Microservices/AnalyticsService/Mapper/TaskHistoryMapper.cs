using SharedLibrary.Dapper.DapperRepositories;
using SharedLibrary.Dapper.DapperRepositories.Abstractions;
using SharedLibrary.Entities.AnalyticsService;
using SharedLibrary.Entities.UserService;
using SharedLibrary.Models;

namespace AnalyticsService.Mapper
{
    public static class TaskHistoryMapper
    {

        public static async Task<TaskHistoryModel?> ToModel(TaskHistoryEntity? entity, IUserRepository userRepository)
        {
            if (entity is null)
                return null;

            var model = new TaskHistoryModel()
            {
                Id = entity.Id,
                ChangedAt = entity.ChangedAt,
                FieldName = entity.FieldName,
                ItemId = entity.ItemId,
                NewValue = entity.NewValue,
                OldValue = entity.OldValue,
                UserId = entity.UserId
            };

            if (entity.UserId != -1)
            {
                var user = await userRepository.GetUserAsync(entity.UserId);
                model.SetUserName(user!.Username);
            }

            return model;
        }
    }
}
