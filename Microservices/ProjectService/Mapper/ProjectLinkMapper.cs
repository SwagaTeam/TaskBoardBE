using SharedLibrary.Dapper.DapperRepositories.Abstractions;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.Mapper;

public class ProjectLinkMapper
{
    public static ProjectLinkEntity ToEntity(ProjectLinkModel model)
    {
        return new ProjectLinkEntity
        {
            ProjectId = model.ProjectId,
            Url = model.Url,
        };
    }
    
    public static async Task<ProjectLinkModel> ToModel(ProjectLinkEntity model, IUserRepository userRepository)
    {
        return new ProjectLinkModel
        {
            Id = model.Id,
            ProjectId = model.ProjectId,
            Project = await ProjectMapper.ToModel(model.Project, userRepository),
            Url = model.Url,
        };
    }
}