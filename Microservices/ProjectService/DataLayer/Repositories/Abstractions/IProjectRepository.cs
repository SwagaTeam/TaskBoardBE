using SharedLibrary.ProjectModels;

namespace ProjectService.DataLayer.Repositories.Abstractions
{
    public interface IProjectRepository
    {
        Task<ProjectModel?> GetById(int id);
        Task<ProjectModel?> GetByBoardId(int id);
        Task<bool> IsUserInProject(int userId, int projectId);
        Task<int> AddUserInProject(int userId, int projectId);
        Task<bool> IsUserAdmin(int userId, int projectId);
        Task<bool> IsUserCanView(int userId, int projectId);
        Task<int> Create(ProjectModel project, int userId);
        Task<int> Update(ProjectModel project);
        Task<int> Delete(int id);
    }
}
