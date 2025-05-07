using SharedLibrary.Auth;
using SharedLibrary.UserModels;
using UserService.DataLayer.Repositories.Abstractions;
using UserService.ViewModels;

namespace UserService.BusinessLayer.Manager
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncrypt _encrypt;

        public UserManager(IUserRepository userRepository, IEncrypt encrypt)
        {
            _userRepository = userRepository;
            _encrypt = encrypt;
        }

        public async Task<int> Create(RegisterModel user)
        {
            var existingUser = await _userRepository.GetByEmail(user.Email);

            if (existingUser != null)
                throw new InvalidOperationException("User with such Email already exist");

            var userModel = new UserModel();
            userModel.Username = user.Username;
            userModel.Email = user.Email;
            userModel.Salt = Guid.NewGuid().ToString();
            userModel.Password = _encrypt.HashPassword(user.Password, userModel.Salt);

            var id = await _userRepository.Create(userModel);

            return id;
        }

        public async Task SetUserAvatar(int userId, IFormFile avatar)
        {
            var avatarPath = Environment.GetEnvironmentVariable("AVATAR_STORAGE_PATH");

            if (string.IsNullOrEmpty(avatarPath))
                throw new Exception("Переменная окружения AVATAR_STORAGE_PATH не задана");

            Directory.CreateDirectory(avatarPath);

            var uniqueFileName = $"{Guid.NewGuid()}_{avatar.FileName}";
            var filePath = Path.Combine(avatarPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await avatar.CopyToAsync(stream);
            }

            var imagePath = $"/avatars/{uniqueFileName}";

            await _userRepository.SetUserAvatar(userId, imagePath);
        }

        public Task<int> Delete(int id)
        {
            return _userRepository.Delete(id);
        }

        public Task<IEnumerable<UserModel>> GetAll()
        {
            return _userRepository.GetAll();
        }

        public Task<UserModel?> GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public Task<UserModel?> GetByEmail(string email)
        {
            return _userRepository.GetByEmail(email);
        }

        public Task<int> Update(UserModel user)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel?> ValidateCredentials(string email, string password)
        {
            var user = await _userRepository.GetByEmail(email);

            if (user == null)
            {
                throw new Exception("Почта пользователя не найдена");

            }

            var hashedPassword = _encrypt.HashPassword(password, user.Salt);

            if (user.Password != hashedPassword)
            {
                return null;
            }

            return user;
        }
    }
}
