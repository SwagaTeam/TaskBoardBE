using SharedLibrary.Entities.ProjectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.ProjectModels
{
    public class SprintModel
    {
        public int Id { get; set; }
        public int BoardId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [JsonIgnore]
        public ICollection<ItemModel> Items { get; set; }

        [JsonIgnore]
        public BoardEntity Board { get; set; }
    }
}
