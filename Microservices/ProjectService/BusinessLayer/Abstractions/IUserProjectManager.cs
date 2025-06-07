using SharedLibrary.Entities.ProjectService;
using SharedLibrary.Models;

namespace ProjectService.BusinessLayer.Abstractions;

public interface IUserProjectManager
{
    public Task CreateAsync(UserProjectModel userProject);
    Task<bool> IsUserInProjectAsync(int userId, int projectId);
    Task<bool> IsUserAdminAsync(int userId, int projectId);
    Task<bool> IsUserCanViewAsync(int userId, int projectId);
    public Task<bool> IsUserViewerAsync(int userId, int projectId);

}