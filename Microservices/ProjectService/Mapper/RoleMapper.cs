using SharedLibrary.Entities.ProjectService;
using SharedLibrary.Models;

namespace ProjectService.Mapper
{
    public static class RoleMapper
    {
        public static RoleEntity ToEntity(RoleModel roleModel)
        {
            return new RoleEntity()
            {
                Role = roleModel.Role,
            };
        }

        public static RoleModel? ToModel(RoleEntity? roleEntity)
        {
            if(roleEntity == null)
                return null;
            return new RoleModel()
            {
                Id = roleEntity.Id,
                Role = roleEntity.Role,
            };
        }
    }
}
