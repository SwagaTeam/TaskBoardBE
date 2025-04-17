using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.ProjectModels
{
    public class JoinRequest
    {
        public string Url { get; set; } = default!;
        public int UserId { get; set; }
    }
}
