using SharedLibrary.Constants;
using SharedLibrary.Dapper.DapperRepositories;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;

namespace ProjectService.Mapper
{
    public static class ProjectMapper
    {
        public static ProjectEntity ToEntity(ProjectModel projectModel)
        {
            return new ProjectEntity()
            {
                Name = projectModel.Name,
                Key = projectModel.Key,
                Description = projectModel.Description,
                ExpectedEndDate = projectModel.ExpectedEndDate,
                IsPrivate = projectModel.IsPrivate,
                Priority = projectModel.Priority,
                StartDate = projectModel.StartDate,
                UpdatedAt = projectModel.UpdateDate
            };
        }

        public static async Task<ProjectModel> ToModel(ProjectEntity projectModel)
        {
            var project = new ProjectModel()
            {
                Id = projectModel.Id,
                Name = projectModel.Name,
                Key = projectModel.Key,
                Description = projectModel.Description,
                ExpectedEndDate = projectModel.ExpectedEndDate,
                IsPrivate = projectModel.IsPrivate,
                Priority = projectModel.Priority,
                StartDate = projectModel.StartDate,
                UpdateDate = projectModel.UpdatedAt,
            };

            var headId = projectModel.UserProjects.Where(x => x.RoleId == DefaultRoles.CREATOR).FirstOrDefault().UserId;
            var user = await UserRepository.GetUser(headId);

            project.SetHead(user.Username);

            return project;
        }
    }
}
