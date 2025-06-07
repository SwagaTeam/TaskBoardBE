using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Mapper;
using SharedLibrary.Models;

namespace ProjectService.BusinessLayer.Implementations;

public class UserProjectManager(IUserProjectRepository repository) : IUserProjectManager
{
    public async Task CreateAsync(UserProjectModel userProject)
    {
        await repository.CreateAsync(UserProjectMapper.ToEntity(userProject));
    }

    public async Task<bool> IsUserInProjectAsync(int userId, int projectId)
    {
        return await repository.IsUserInProjectAsync(userId, projectId);
    }

    public async Task<bool> IsUserAdminAsync(int userId, int projectId)
    {
        return await repository.IsUserAdminAsync(userId, projectId);
    }

    public async Task<bool> IsUserCanViewAsync(int userId, int projectId)
    {
        return await repository.IsUserCanViewAsync(userId, projectId);
    }

    public async Task<bool> IsUserViewerAsync(int userId, int projectId)
    {
        return await repository.IsUserViewerAsync(userId, projectId);
    }
}