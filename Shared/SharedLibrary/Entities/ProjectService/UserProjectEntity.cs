using SharedLibrary.Entities.UserService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Entities.ProjectService
{
    public class UserProjectEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public int Privilege { get; set; }
        [DefaultValue(null)]
        public int? RoleId { get; set; }

        public ProjectEntity Project { get; set; }
        public RoleEntity Role { get; set; }
    }
}
