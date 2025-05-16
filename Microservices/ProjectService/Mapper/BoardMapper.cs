using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;

namespace ProjectService.Mapper
{
    public static class BoardMapper
    {
        public static BoardEntity ToEntity(BoardModel model)
        {
            return new BoardEntity
            {
                Name = model.Name,
                Description = model.Description,
                CreatedAt = model.CreatedAt,
                ProjectId = model.ProjectId,
            };
        }

        public static BoardModel ToModel(BoardEntity entity)
        {
            var model = new BoardModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt,
                ProjectId = entity.ProjectId,
                Project = entity.Project is null ? null : ProjectMapper.ToModel(entity.Project),
                //Sprints = SprintsMapper.ToModel(model.Sprints),
                Statuses = entity.Statuses.Select(StatusMapper.ToModel).ToList()
            };

            model.SetItemsCount(entity.ItemsBoards.Count);

            return model;
        }
    }
}
