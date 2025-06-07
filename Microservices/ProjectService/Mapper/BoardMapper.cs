using SharedLibrary.Dapper.DapperRepositories.Abstractions;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.Models;
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

        public static async Task<BoardModel> ToModel(BoardEntity entity, IUserRepository userRepository)
        {
            var model = new BoardModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt,
                ProjectId = entity.ProjectId,
                Project = entity.Project is null ? null : await ProjectMapper.ToModel(entity.Project, userRepository),
                //Sprints = SprintsMapper.ToModel(model.Sprints),
                Statuses = entity.Statuses.Select(StatusMapper.ToModel).ToList()
            };

            model.SetItemsCount(entity.ItemsBoards.Count);
            model.SetProjectName(entity.Project.Name);

            return model;
        }
    }
}
