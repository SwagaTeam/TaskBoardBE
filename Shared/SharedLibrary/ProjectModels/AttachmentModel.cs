using SharedLibrary.Entities.ProjectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.ProjectModels
{
    public class AttachmentModel
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int CommentId { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }

        [JsonIgnore]
        public CommentModel Comment { get; set; }
    }
}
