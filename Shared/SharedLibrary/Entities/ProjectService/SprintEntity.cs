using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Entities.ProjectService
{
    public class SprintEntity
    {
        public int Id { get; set; }
        public int BoardId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<ItemEntity> Items { get; set; }
        public BoardEntity Board { get; set; }
    }
}
