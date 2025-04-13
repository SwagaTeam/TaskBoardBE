using SharedLibrary.Entities.UserService;
using SharedLibrary.UserModels;

namespace UserService.BusinessLayer
{
    public static class UserMapper
    {
        public static UserModel UserEntityToUserModel (UserEntity userEntity)
        {
            return new UserModel()
            {
                Id = userEntity.Id,
                Username = userEntity.Username,
                Email = userEntity.Email,
                Password = userEntity.Password,
                Salt = userEntity.Salt
            };
        }

        public static UserEntity UserModelToUserEntity(UserModel userEntity)
        {
            return new UserEntity()
            {
                Username = userEntity.Username,
                Email = userEntity.Email,
                Password = userEntity.Password,
                Salt = userEntity.Salt
            };
        }
    }
}
