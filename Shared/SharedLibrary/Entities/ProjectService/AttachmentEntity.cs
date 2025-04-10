using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Entities.UserService;

namespace SharedLibrary.Entities.ProjectService
{
    public class AttachmentEntity
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int CommentId { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
        public UserEntity User { get; set; }
        public CommentEntity Comment { get; set; }
    }
}
