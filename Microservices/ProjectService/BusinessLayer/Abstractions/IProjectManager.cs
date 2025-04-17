using SharedLibrary.ProjectModels;

namespace ProjectService.BusinessLayer.Abstractions
{
    public interface IProjectManager
    {
        Task<ProjectModel?> GetById(int id);
        Task<bool> IsUserInProject(int userId, int projectId);
        Task<int> AddUserInProject(int userId, int projectId);
        Task<int> Create(ProjectModel project);
        Task<int> Update(ProjectModel project);
        Task<int> Delete(int id);
    }
}
