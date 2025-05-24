using SharedLibrary.UserModels;

namespace SharedLibrary.Dapper.DapperRepositories.Abstractions;

public interface IUserRepository
{
    Task<UserModel?> GetUserAsync(int id);
    Task<UserModel?> GetUserByEmailAsync(string email);
}