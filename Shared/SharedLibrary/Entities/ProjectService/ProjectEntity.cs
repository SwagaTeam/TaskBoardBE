using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Entities.ProjectService
{
    public class ProjectEntity
    {
        public int Id { get; set; }
        public string Key {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public int Priority { get; set; }

        public virtual ICollection<BoardEntity> Boards { get; set; }
        public virtual ICollection<ItemEntity> Items { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserProjectEntity> UserProjects { get; set; }
        public virtual ICollection<DocumentEntity> Documents { get; set; }
        public virtual ICollection<VisibilityLinkEntity> VisibilityLinks { get; set; }
    }

}
