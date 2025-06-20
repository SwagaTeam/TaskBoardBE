using SharedLibrary.Models;
using SharedLibrary.ProjectModels;

namespace ProjectService.BusinessLayer.Abstractions
{
    public interface IProjectLinkManager
    {
        Task<string> CreateAsync(int projectId, int userId);
        Task<ProjectLinkModel?> GetByIdAsync(int id);
        Task<ProjectLinkModel?> GetByLinkAsync(string link);
    }
}
