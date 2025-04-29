using SharedLibrary.Entities.ProjectService;

namespace ProjectService.Mapper;

public class UserProjectMapper
{
    public static UserProjectEntity ToEntity(UserProjectModel model)
    {
        return new UserProjectEntity
        {
            Id = model.Id,
            ProjectId = model.ProjectId,
            Privilege = model.Privilege,
            RoleId = model.RoleId,
            UserId = model.UserId,
        };
    }
    
    public static UserProjectModel ToModel(UserProjectEntity model)
    {
        return new UserProjectModel
        {
            Id = model.Id,
            ProjectId = model.ProjectId,
            Privilege = model.Privilege,
            RoleId = model.RoleId,
            UserId = model.UserId,
        };
    }
}