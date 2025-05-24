using SharedLibrary.Dapper.DapperRepositories.Abstractions;
using SharedLibrary.UserModels;

namespace SharedLibrary.Dapper.DapperRepositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<UserModel?> GetUserAsync(int id)
        {
            var query = "SELECT * FROM \"Users\" WHERE \"Id\" = @Id";
            var result = await DapperOperations.QueryAsync<UserModel>(query, new { Id = id });
            return result.FirstOrDefault();
        }

        public async Task<UserModel?> GetUserByEmailAsync(string email)
        {
            var query = "SELECT * FROM \"Users\" WHERE \"Email\" = @Email";
            var result = await DapperOperations.QueryAsync<UserModel>(query, new { Email = email });
            return result.FirstOrDefault();
        }
    }

    
}