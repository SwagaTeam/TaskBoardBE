using SharedLibrary.Entities.ProjectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.ProjectModels
{
    public class ItemTypeModel
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public ICollection<ItemModel> Items { get; set; }
    }
}
