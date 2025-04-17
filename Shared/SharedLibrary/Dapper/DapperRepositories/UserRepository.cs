using SharedLibrary.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Dapper.DapperRepositories
{
    public static class UserRepository
    {
        public static async Task<UserModel> GetUser(int id)
        {
            var result = await DapperOperations.QueryAsync<UserModel>($"select * from \"Users\" u where \"Id\" = {id}", new { Id = id });
            return result.FirstOrDefault() ?? new UserModel();
        }
    }
}
