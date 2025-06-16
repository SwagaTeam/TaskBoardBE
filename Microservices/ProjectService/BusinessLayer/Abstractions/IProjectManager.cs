using ProjectService.Models;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.Models;
using SharedLibrary.ProjectModels;
using SharedLibrary.UserModels;

namespace ProjectService.BusinessLayer.Abstractions
{
    public interface IProjectManager
    {
        Task<ICollection<ProjectModel?>> Get();
        Task<ProjectModel?> GetByIdAsync(int id);
        Task<ProjectModel?> GetByBoardIdAsync(int id);
        Task<bool> IsUserInProjectAsync(int userId, int projectId);
        public Task<IEnumerable<UserDto>> GetUsersInProjectAsync(int projectId);
        Task<bool> IsUserCanViewAsync(int userId, int projectId);
        Task<int> AddUserInProjectAsync(int userId, int projectId);
        Task<bool> IsUserAdminAsync(int userId, int projectId);
        Task<bool> IsUserViewerAsync(int userId, int projectId);
        Task<int> SetUserRoleAsync(int userId, int projectId, RoleModel role);
        Task<int> CreateAsync(ProjectModel project);
        Task<ProjectModel> UpdateAsync(ProjectModel project);
        Task DeleteAsync(int id);
        Task<TasksState> GetTasksStateAsync(int projectId);
    }
}
