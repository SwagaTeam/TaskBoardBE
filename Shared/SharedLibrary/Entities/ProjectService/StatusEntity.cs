using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Entities.ProjectService
{
    public class StatusEntity
    {
        public int Id { get; set; }
        public int BoardId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; }
        public bool IsDone { get; set; }
        public bool IsRejected { get; set; }
        public virtual ICollection<ItemEntity> Items { get; set; }
        public virtual BoardEntity Board { get; set; }
        public virtual ICollection<ItemBoardEntity> ItemsBoards { get; set; } = new List<ItemBoardEntity>();
    }
}
