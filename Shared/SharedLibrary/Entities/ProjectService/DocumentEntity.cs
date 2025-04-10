using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Entities.UserService;

namespace SharedLibrary.Entities.ProjectService
{
    public class DocumentEntity
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        public string Description { get; set; } = string.Empty;

        public ProjectEntity Project { get; set; } = null!;
        public UserEntity User { get; set; }
    }
}
