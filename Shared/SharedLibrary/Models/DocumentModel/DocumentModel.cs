using SharedLibrary.Entities.ProjectService;
using SharedLibrary.Entities.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models.DocumentModel
{
    public class DocumentModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
