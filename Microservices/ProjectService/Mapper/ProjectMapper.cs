using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;

namespace ProjectService.Mapper
{
    public static class ProjectMapper
    {
        public static ProjectEntity ProjectModelToProjectEntity(ProjectModel projectModel)
        {
            return new ProjectEntity()
            {
                Name = projectModel.Name,
                Key = projectModel.Key,
                CreatedAt = projectModel.CreatedAt,
                UpdatedAt = projectModel.UpdatedAt,
                Description = projectModel.Description,
                ExpectedEndDate = projectModel.ExpectedEndDate,
                IsPrivate = projectModel.IsPrivate,
                Priority = projectModel.Priority,
                StartDate = projectModel.StartDate
            };
        }

        public static ProjectModel ProjectEntityToProjectModel(ProjectEntity projectModel)
        {
            return new ProjectModel()
            {
                Name = projectModel.Name,
                Key = projectModel.Key,
                CreatedAt = projectModel.CreatedAt,
                UpdatedAt = projectModel.UpdatedAt,
                Description = projectModel.Description,
                ExpectedEndDate = projectModel.ExpectedEndDate,
                IsPrivate = projectModel.IsPrivate,
                Priority = projectModel.Priority,
                StartDate = projectModel.StartDate
            };
        }
    }
}
