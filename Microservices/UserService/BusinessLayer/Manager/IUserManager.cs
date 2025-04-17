using SharedLibrary.UserModels;

namespace UserService.BusinessLayer.Manager
{
    public interface IUserManager
    {
        Task<int> Create(UserModel user);
        Task<int> Update(UserModel user);
        Task<int> Delete(int id);
        Task<UserModel?> GetById(int id);
        Task<UserModel?> GetByEmail(string email);
        Task<IEnumerable<UserModel>> GetAll();
        Task<UserModel?> ValidateCredentials(string email, string password);
    }
}
