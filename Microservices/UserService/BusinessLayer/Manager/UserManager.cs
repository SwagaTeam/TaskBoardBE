using SharedLibrary.Auth;
using SharedLibrary.UserModels;
using UserService.DataLayer.Repositories.Abstractions;

namespace UserService.BusinessLayer.Manager
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository userRepository;
        private readonly IEncrypt encrypt;

        public UserManager(IUserRepository userRepository, IEncrypt encrypt)
        {
            this.userRepository = userRepository;
            this.encrypt = encrypt;
        }

        public async Task<int> Create(UserModel user)
        {
            var existingUser = await userRepository.GetByEmail(user.Username);

            if (existingUser != null)
                throw new InvalidOperationException("User with such Username already exist");

            user.Salt = Guid.NewGuid().ToString();
            user.Password = encrypt.HashPassword(user.Password, user.Salt);

            var id = await userRepository.Create(user);

            return id;
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<UserModel?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel?> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(UserModel user)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel?> ValidateCredentials(string email, string password)
        {
            var user = await userRepository.GetByEmail(email);

            if (user == null)
            {
                throw new Exception("Почта пользователя не найдена");

            }

            var hashedPassword = encrypt.HashPassword(password, user.Salt);

            if (user.Password != hashedPassword)
            {
                return null;
            }

            return user;
        }
    }
}
