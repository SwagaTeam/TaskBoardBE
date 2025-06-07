using SharedLibrary.Entities.ProjectService;
using SharedLibrary.Models;

namespace ProjectService.Mapper;

public static class UserProjectMapper
{
    public static UserProjectEntity ToEntity(UserProjectModel model)
    {
        return new UserProjectEntity
        {
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
            Role = RoleMapper.ToModel(model.Role),
        };
    }
}