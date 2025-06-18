using SharedLibrary.Dapper.DapperRepositories.Abstractions;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.Models;

namespace ProjectService.Mapper;

public static class ProjectLinkMapper
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
        var id = model.Id;
        var pId = model.ProjectId;
        var project = await ProjectMapper.ToModel(model.Project, userRepository);
        var url = model.Url;
        return new ProjectLinkModel
        {
            Id = model.Id,
            ProjectId = model.ProjectId,
            Project = await ProjectMapper.ToModel(model.Project, userRepository),
            Url = model.Url,
        };
    }
}