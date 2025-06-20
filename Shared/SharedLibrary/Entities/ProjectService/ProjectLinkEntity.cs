using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Entities.ProjectService
{
    public class ProjectLinkEntity
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Url { get; set; } = string.Empty;
        public int UserId { get; set; }

        public ProjectEntity Project { get; set; }
    }
}
