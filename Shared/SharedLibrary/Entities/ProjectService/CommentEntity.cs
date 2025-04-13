using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Entities.UserService;

namespace SharedLibrary.Entities.ProjectService
{
    public class CommentEntity
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int ItemId { get; set; }
        public string Text { get; set; } = "";
        public DateTime CreatedAt { get; set; }

        public List<AttachmentEntity> Attachments { get; set; } = new List<AttachmentEntity>();
        public UserEntity User { get; set; } = new UserEntity();
        public ItemEntity Item { get; set; } = new ItemEntity();
    }
}
