using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;

namespace ProjectService.Mapper
{
    public static class StatusMapper
    {
        public static StatusModel ToModel(StatusEntity statusEntity)
        {
            return new StatusModel
            {
                Order = statusEntity.Order,
                Boards = statusEntity.Boards.Select(BoardMapper.ToModel).ToList(),
                IsDone = statusEntity.IsDone,
                IsRejected = statusEntity.IsRejected,
                Name = statusEntity.Name,
                //Items = statusEntity.Items
            };
        }

        public static StatusEntity ToEntity(StatusModel statusEntity)
        {
            return new StatusEntity
            {
                Order = statusEntity.Order,
                Boards = statusEntity.Boards.Select(BoardMapper.ToEntity).ToList(),
                IsDone = statusEntity.IsDone,
                IsRejected = statusEntity.IsRejected,
                Name = statusEntity.Name,
                //Items = statusEntity.Items
            };
        }
    }
}
