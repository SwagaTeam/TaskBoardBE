using SharedLibrary.Entities.ProjectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.ProjectModels
{
    public class CommentModel
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int ItemId { get; set; }
        public string Text { get; set; } = "";
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public ItemModel Item { get; set; }

        [JsonIgnore]
        public ICollection<AttachmentModel> Attachments { get; set; }
    }
}
