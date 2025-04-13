using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Auth
{
    public interface IAuth
    {
        public string GenerateJwtToken<T>(T user);
        int? GetCurrentUserId();
        void Logout(string token);
        List<string> GetCurrentUserRoles();
    }
}
