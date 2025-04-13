using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Auth
{
    public interface IEncrypt
    {
        public string HashPassword(string password, string salt);
    }
}
