using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;

namespace ProjectService.BusinessLayer.Abstractions
{
    public interface IProjectManager
    {
        Task<ProjectModel?> GetByIdAsync(int id);
        Task<ProjectModel?> GetByBoardIdAsync(int id);
        Task<bool> IsUserInProjectAsync(int userId, int projectId);
        Task<bool> IsUserCanViewAsync(int userId, int projectId);
        Task<int> AddUserInProjectAsync(int userId, int projectId);
        Task<bool> IsUserAdminAsync(int userId, int projectId);
        Task<bool> IsUserViewerAsync(int userId, int projectId);
        Task<int> CreateAsync(ProjectModel project);
        Task UpdateAsync(ProjectModel project);
        Task DeleteAsync(int id);
    }
}
