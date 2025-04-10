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
        public int BoardId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public int Priority { get; set; }
        public bool IsBug { get; set; }
        public int DepthLevel { get; set; }
        public int StatusId { get; set; }
        public bool IsArchived { get; set; }

        public List<CommentEntity> Comments { get; set; }
        public List<AttachmentEntity> Attachments { get; set; }
    }
}
