using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Entities.ProjectService
{
    public class BoardEntity
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int StatusId { get; set; }

        public ProjectEntity Project { get; set; }

        public StatusEntity Status { get; set; }

        public ICollection<SprintEntity> Sprints { get; set; }
        public virtual ICollection<ItemBoardEntity> ItemsBoards { get; set; } = new List<ItemBoardEntity>();
    }
}
