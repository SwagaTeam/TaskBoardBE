using SharedLibrary.Entities.ProjectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.ProjectModels
{
    public class StatusModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; }
        public bool IsDone { get; set; }
        public bool IsRejected { get; set; }

        [JsonIgnore]
        public ICollection<ItemModel> Items { get; set; }
        [JsonIgnore]
        public ICollection<BoardModel> Boards { get; set; }
    }
}
