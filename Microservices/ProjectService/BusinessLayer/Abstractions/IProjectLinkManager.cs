﻿using SharedLibrary.ProjectModels;

namespace ProjectService.BusinessLayer.Abstractions
{
    public interface IProjectLinkManager
    {
        Task<string> CreateAsync(int projectId);
        Task<ProjectLinkModel?> GetByIdAsync(int id);
        Task<ProjectLinkModel?> GetByLinkAsync(string link);
    }
}
