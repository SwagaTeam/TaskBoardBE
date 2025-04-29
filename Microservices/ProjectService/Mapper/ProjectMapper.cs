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
                StartDate = projectModel.StartDate
            };
        }

        public static ProjectModel ToModel(ProjectEntity projectModel)
        {
            return new ProjectModel()
            {
                Id = projectModel.Id,
                Name = projectModel.Name,
                Key = projectModel.Key,
                Description = projectModel.Description,
                ExpectedEndDate = projectModel.ExpectedEndDate,
                IsPrivate = projectModel.IsPrivate,
                Priority = projectModel.Priority,
                StartDate = projectModel.StartDate
            };
        }
    }
}
