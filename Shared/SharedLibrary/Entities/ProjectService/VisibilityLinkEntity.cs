﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Entities.ProjectService
{
    public class VisibilityLinkEntity
    {
        public int ProjectId { get; set; }
        public string Url { get; set; } = string.Empty;

        public ProjectEntity Project { get; set; }
    }
}
