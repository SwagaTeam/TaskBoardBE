using SharedLibrary.Entities.ProjectService;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.ProjectModels
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public int Priority { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserProjectModel> UserProjects { get; set; } = new List<UserProjectModel>();
    }
}
