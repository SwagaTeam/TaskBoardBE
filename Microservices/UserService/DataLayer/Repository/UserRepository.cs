using Microsoft.EntityFrameworkCore;
using SharedLibrary.Entities.UserService;
using SharedLibrary.UserModels;
using System.Reflection.Metadata.Ecma335;
using UserService.BusinessLayer;
using UserService.DataLayer.Repositories.Abstractions;

namespace UserService.DataLayer.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _userDbContext;

        public UserRepository(UserDbContext userDbContext) 
        {
            _userDbContext = userDbContext;
        }

        public async Task<int> Create(UserModel user)
        {
            var userEntity = UserMapper.UserModelToUserEntity(user);
            await _userDbContext.Users.AddAsync(userEntity);
            await _userDbContext.SaveChangesAsync();

            return userEntity.Id;
        }

        public async Task<int> Delete(int id)
        {
            var existingUser = await _userDbContext.Users.FindAsync(id);

            if (existingUser is not null)
            {
                _userDbContext.Users.Remove(existingUser);
                await _userDbContext.SaveChangesAsync();
                return existingUser.Id;
            }

            throw new Exception($"User with id {id} not found");
        }

        public async Task<IEnumerable<UserModel>> GetAll()
        {
            var users = await _userDbContext.Users
                .AsNoTracking()
                .ToListAsync();

            if (users.Any())
                return users.Select(UserMapper.UserEntityToUserModel);

            return Enumerable.Empty<UserModel>();
        }

        public async Task<UserModel?> GetById(int id)
        {
            var user = await _userDbContext.Users.FindAsync(id);

            if (user is not null)
                return UserMapper.UserEntityToUserModel(user);

            return null;
        }

        public async Task<UserModel?> GetByEmail(string email)
        {
            var user = await _userDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user is not null)
                return UserMapper.UserEntityToUserModel(user);

            return null;
        }

        public async Task<int> Update(UserModel user)
        {
            var userEntity = await _userDbContext.Users.FindAsync(user.Id);

            userEntity!.Username = user.Username;
            userEntity.Password = user.Password;
            userEntity.Salt = user.Salt;
            userEntity.Email = user.Email;

            await _userDbContext.SaveChangesAsync();

            return userEntity.Id;
        }

        public async Task SetUserAvatar(int userId, string path)
        {
            var userEntity = await _userDbContext.Users.FindAsync(userId);

            if(userEntity is not null)
            {
                userEntity.ImagePath = path;
                await _userDbContext.SaveChangesAsync();
                return;
            }

            throw new Exception("Пользователь не найден");
        }
    }
}
