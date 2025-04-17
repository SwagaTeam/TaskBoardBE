using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Entities.ProjectService
{
    public class ItemTypeEntity
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public ICollection<ItemEntity> Items { get; set; }
    }
}
