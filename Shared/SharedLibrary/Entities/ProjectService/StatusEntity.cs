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
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; }
        public bool IsDone { get; set; }
        public bool IsRejected { get; set; }
        public ICollection<ItemEntity> Items { get; set; }

        public ICollection<BoardEntity> Boards { get; set; }
    }
}
