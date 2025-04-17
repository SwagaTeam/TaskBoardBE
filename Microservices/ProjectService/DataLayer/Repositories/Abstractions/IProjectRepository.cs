using SharedLibrary.ProjectModels;

namespace ProjectService.DataLayer.Repositories.Abstractions
{
    public interface IProjectRepository
    {
        Task<ProjectModel?> GetById(int id);
        Task<bool> IsUserInProject(int userId, int projectId);
        Task<int> AddUserInProject(int userId, int projectId);
        Task<int> Create(ProjectModel project, int userId);
        Task<int> Update(ProjectModel project);
        Task<int> Delete(int id);
    }
}
