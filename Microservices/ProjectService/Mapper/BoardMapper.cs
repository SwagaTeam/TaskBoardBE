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

        public static BoardModel ToModel(BoardEntity model)
        {
            return new BoardModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                CreatedAt = model.CreatedAt,
                ProjectId = model.ProjectId,
                Project = model.Project is null ? null : ProjectMapper.ToModel(model.Project),
                //Sprints = SprintsMapper.ToModel(model.Sprints),
                Statuses = model.Statuses.Select(StatusMapper.ToModel).ToList()
            };
        }
    }
}
