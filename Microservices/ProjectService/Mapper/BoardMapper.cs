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
                StatusId = model.StatusId,
                ProjectId = model.ProjectId,
                Status = model.Status is null ? null : StatusMapper.ToEntity(model.Status),
            };
        }

        public static BoardModel ToModel(BoardEntity model)
        {
            return new BoardModel
            {
                Name = model.Name,
                Description = model.Description,
                CreatedAt = model.CreatedAt,
                StatusId = model.StatusId,
                ProjectId = model.ProjectId,
                Project = model.Project is null ? null : ProjectMapper.ToModel(model.Project),
                //Sprints = SprintsMapper.ToModel(model.Sprints),
                Status = model.Status is null ? null : StatusMapper.ToModel(model.Status)
            };
        }
    }
}
