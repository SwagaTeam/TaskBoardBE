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
    
    public static ProjectLinkModel ToModel(ProjectLinkEntity model)
    {
        return new ProjectLinkModel
        {
            Id = model.Id,
            ProjectId = model.ProjectId,
            Project = ProjectMapper.ToModel(model.Project),
            Url = model.Url,
        };
    }
}