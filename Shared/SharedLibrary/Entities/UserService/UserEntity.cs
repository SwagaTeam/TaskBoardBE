using SharedLibrary.Entities.ProjectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Entities.UserService
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        //public ICollection<UserProjectEntity> UserProjects { get; set; } = new List<UserProjectEntity>();
    }
}
