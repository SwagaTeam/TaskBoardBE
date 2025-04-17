using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Entities.ProjectService
{
    public class ProjectEntity
    {
        public int Id { get; set; }
        public string Key {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IsPrivate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public int Priority { get; set; }

        public ICollection<BoardEntity> Boards { get; set; }
        public ICollection<ItemEntity> Items { get; set; }
        public ICollection<UserProjectEntity> UserProjects { get; set; }
        public ICollection<DocumentEntity> Documents { get; set; }
        public ICollection<VisibilityLinkEntity> VisibilityLinks { get; set; }
    }

}
