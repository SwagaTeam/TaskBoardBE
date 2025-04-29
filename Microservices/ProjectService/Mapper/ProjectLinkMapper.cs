using SharedLibrary.Entities.ProjectService;

namespace ProjectService.Mapper;

public class ProjectLinkMapper
{
    public static ProjectLinkEntity ToEntity(ProjectLinkModel model)
    {
        return new ProjectLinkEntity
        {
            Id = model.Id,
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
            Url = model.Url,
        };
    }
}