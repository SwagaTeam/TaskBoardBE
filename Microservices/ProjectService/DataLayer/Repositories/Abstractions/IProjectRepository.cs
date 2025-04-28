using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;

namespace ProjectService.DataLayer.Repositories.Abstractions
{
    public interface IProjectRepository
    {
        Task<ProjectEntity?> GetById(int id);
        Task<ProjectEntity?> GetByBoardId(int id);
        Task<bool> IsUserInProject(int userId, int projectId);
        Task<int> AddUserInProject(int userId, int projectId);
        Task<bool> IsUserAdmin(int userId, int projectId);
        Task<bool> IsUserCanView(int userId, int projectId);
        Task Create(ProjectEntity project, int userId);
        Task Update(ProjectEntity project);
        Task Delete(int id);
    }
}
