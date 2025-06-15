using SharedLibrary.UserModels;
using UserService.ViewModels;

namespace UserService.BusinessLayer.Manager
{
    public interface IUserManager
    {
        Task<int> Create(RegisterModel user);
        Task<int> Delete(int id);
        Task<UserModel?> GetById(int id);
        Task ChangePassword(int userId, string newPassword);
        Task<UserModel?> GetByEmail(string email);
        Task<IEnumerable<UserModel>> GetAll();
        Task<UserModel?> ValidateCredentials(string email, string password);
        Task SetUserAvatar(int userId, IFormFile avatar);
    }
}