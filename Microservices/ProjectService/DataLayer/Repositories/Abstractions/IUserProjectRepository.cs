using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Abstractions;

public interface IUserProjectRepository
{
    public Task CreateAsync(UserProjectEntity userProject);
    Task<bool> IsUserInProject(int userId, int projectId);
    Task<bool> IsUserAdmin(int userId, int projectId);
    Task<bool> IsUserCanView(int userId, int projectId);
    public Task<bool> IsUserViewer(int userId, int projectId);

}