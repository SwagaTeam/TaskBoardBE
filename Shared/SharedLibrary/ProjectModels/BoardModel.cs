using SharedLibrary.Entities.ProjectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.ProjectModels
{
    public class BoardModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int StatusId { get; set; }

        [JsonIgnore]
        public ProjectModel? Project { get; set; } = null;

        [JsonIgnore]
        public StatusModel? Status { get; set; } = null;

        [JsonIgnore]
        public ICollection<SprintModel>? Sprints { get; set; } = null;
    }
}
