using SharedLibrary.Entities.UserService;
using SharedLibrary.UserModels;

namespace UserService.DataLayer.Repositories.Abstractions
{
    public interface IUserRepository
    {
        Task<int> Create(UserModel user);
        Task<int> Update(UserModel user);
        Task<int> Delete(int id);
        Task<UserModel?> GetById(int id);
        Task<UserModel?> GetByEmail(string email);
        Task<IEnumerable<UserModel>> GetAll();
        Task SetUserAvatar(int userId, string path);
    }
}
