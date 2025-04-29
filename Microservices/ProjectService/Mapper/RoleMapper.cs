using SharedLibrary.Entities.ProjectService;

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

        public static RoleModel ToModel(RoleEntity roleEntity)
        {
            return new RoleModel()
            {
                Role = roleEntity.Role,
            };
        }
    }
}
