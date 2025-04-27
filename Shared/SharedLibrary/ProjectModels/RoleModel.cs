using SharedLibrary.Entities.ProjectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.ProjectModels
{
    public class RoleModel
    {
        public int Id { get; set; }
        public string Role { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserProjectModel> UserProjects { get; set; }
    }
}
