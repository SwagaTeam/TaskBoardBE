using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Mapper;

namespace ProjectService.BusinessLayer.Implementations;

public class UserProjectManager(IUserProjectRepository repository) : IUserProjectManager
{
    public async Task CreateAsync(UserProjectModel userProject)
    {
        await repository.CreateAsync(UserProjectMapper.ToEntity(userProject));
    }

    public async Task<bool> IsUserInProjectAsync(int userId, int projectId)
    {
        return await repository.IsUserInProject(userId, projectId);
    }

    public async Task<bool> IsUserAdminAsync(int userId, int projectId)
    {
        return await repository.IsUserAdmin(userId, projectId);
    }

    public async Task<bool> IsUserCanViewAsync(int userId, int projectId)
    {
        return await repository.IsUserCanView(userId, projectId);
    }

    public async Task<bool> IsUserViewerAsync(int userId, int projectId)
    {
        return await repository.IsUserViewer(userId, projectId);
    }
}