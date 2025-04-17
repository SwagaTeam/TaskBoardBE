using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.ProjectModels
{
    public class ProjectLinkModel
    {
        public int ProjectId { get; set; }
        public string URL { get; set; } = "";
        public ProjectModel? Project;
    }
}
