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
        private readonly UserDbContext userDbContext;

        public UserRepository(UserDbContext userDbContext) 
        {
            this.userDbContext = userDbContext;
        }

        public async Task<int> Create(UserModel user)
        {
            var userEntity = UserMapper.UserModelToUserEntity(user);
            await userDbContext.Users.AddAsync(userEntity);
            await userDbContext.SaveChangesAsync();

            return userEntity.Id;
        }

        public async Task<int> Delete(int id)
        {
            var existingUser = await userDbContext.Users.FindAsync(id);

            if (existingUser is not null)
            {
                userDbContext.Users.Remove(existingUser);
                await userDbContext.SaveChangesAsync();
                return existingUser.Id;
            }

            throw new Exception($"User with id {id} not found");
        }

        public async Task<IEnumerable<UserModel>> GetAll()
        {
            var users = await userDbContext.Users
                .AsNoTracking()
                .ToListAsync();

            if (users.Any())
                return users.Select(UserMapper.UserEntityToUserModel);

            return Enumerable.Empty<UserModel>();
        }

        public async Task<UserModel?> GetById(int id)
        {
            var user = await userDbContext.Users.FindAsync(id);

            if (user is not null)
                return UserMapper.UserEntityToUserModel(user);

            return null;
        }

        public async Task<UserModel?> GetByEmail(string email)
        {
            var user = await userDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user is not null)
                return UserMapper.UserEntityToUserModel(user);

            return null;
        }

        public async Task<int> Update(UserModel user)
        {
            var userEntity = await GetById(user.Id);

            userEntity!.Username = user.Username;
            userEntity.Password = user.Password;
            userEntity.Salt = user.Salt;
            userEntity.Email = user.Email;

            await userDbContext.SaveChangesAsync();

            return userEntity.Id;
        }
    }
}
