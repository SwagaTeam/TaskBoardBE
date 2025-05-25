using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Abstractions;

public interface IUserProjectRepository
{
    public Task CreateAsync(UserProjectEntity userProject);
    Task<bool> IsUserInProjectAsync(int userId, int projectId);
    Task<bool> IsUserAdminAsync(int userId, int projectId);
    Task<bool> IsUserCanViewAsync(int userId, int projectId);
    public Task<bool> IsUserViewerAsync(int userId, int projectId);
}