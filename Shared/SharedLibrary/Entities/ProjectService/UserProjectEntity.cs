using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Entities.ProjectService
{
    public class UserProjectEntity
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public int Role { get; set; }

        public ProjectEntity Project { get; set; } = null!;
    }
}
