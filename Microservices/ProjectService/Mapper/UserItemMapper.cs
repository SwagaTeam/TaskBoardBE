using SharedLibrary.Entities.ProjectService;
using SharedLibrary.Models;

namespace ProjectService.Mapper
{
    public static class UserItemMapper
    {
        public static UserItemModel ToModel(UserItemEntity entity)
        {
            return new UserItemModel()
            {
                ItemId = entity.ItemId,
                UserId = entity.UserId
            };
        }

        public static UserItemEntity ToEntity(UserItemModel model)
        {
            return new UserItemEntity()
            {
                UserId = model.UserId,
                ItemId = model.ItemId
            };
        }
    }
}
