using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;

namespace ProjectService.BusinessLayer.Abstractions
{
    public interface IProjectManager
    {
        Task<ProjectModel?> GetById(int id);
        Task<ProjectModel?> GetByBoardId(int id);
        Task<bool> IsUserInProject(int userId, int projectId);
        Task<bool> IsUserCanView(int userId, int projectId);
        Task<int> AddUserInProject(int userId, int projectId);
        Task<bool> IsUserAdmin(int userId, int projectId);
        Task<int> Create(ProjectModel project);
        Task Update(ProjectModel project);
        Task Delete(int id);
    }
}
