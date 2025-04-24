using Contributors.BusinessLayer.Abstractions;
using Contributors.DataLayer.Repositories.Abstractions;
using SharedLibrary.Entities.ProjectService;

namespace Contributors.BusinessLayer.Implementations;

public class ContributorsManager(IContributorsRepository contributorsRepository) : IContributorsManager
{
    public async Task<ICollection<UserProjectEntity>> GetUserByProjectIdAsync(int projectId)
    {
        return await contributorsRepository.GetByProjectId(projectId);
    }
}