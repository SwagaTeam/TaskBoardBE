using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.BusinessLayer.Implementations;

public class ContributorsManager(IContributorsRepository contributorsRepository) : IContributorsManager
{
    public async Task<ICollection<UserProjectEntity>> GetUserByProjectIdAsync(int projectId)
    {
        return await contributorsRepository.GetByProjectId(projectId);
    }
}