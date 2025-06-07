using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class ItemCreatedModel
    {
        public ItemModel Item { get; set; }
        public List<int> UserIds { get; set; } = new List<int>();
    }
}
