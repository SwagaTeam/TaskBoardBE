using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SharedLibrary.Entities.ProjectService
{
    public class ItemEntity
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int? ProjectId { get; set; }
        public int? AuthorId { get; set; } = -1;
        public int? ProjectItemNumber { get; set; }
        public string BusinessId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public int Priority { get; set; }
        public int? ItemTypeId { get; set; }
        public int? StatusId { get; set; }
        public bool IsArchived { get; set; }

        public ItemEntity Parent { get; set; }
        public ICollection<ItemEntity> Children { get; set; }

        public ProjectEntity Project { get; set; }

        public ItemTypeEntity ItemType { get; set; }

        public StatusEntity Status { get; set; }

        public ICollection<CommentEntity> Comments { get; set; }
        public ICollection<SprintEntity> Sprints { get; set; }
        public virtual ICollection<ItemBoardEntity> ItemsBoards { get; set; } = new List<ItemBoardEntity>();
        public virtual ICollection<UserItemEntity> UserItems { get; set; } = new List<UserItemEntity>();
    }
}
