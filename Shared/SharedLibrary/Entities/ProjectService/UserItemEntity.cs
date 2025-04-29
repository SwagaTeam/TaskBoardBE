using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Entities.ProjectService
{
    public class UserItemEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }

        public ItemEntity Item { get; set; }
    }
}
