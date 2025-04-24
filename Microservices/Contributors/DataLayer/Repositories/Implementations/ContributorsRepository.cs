using Contributors.DataLayer.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Entities.ProjectService;

namespace Contributors.DataLayer.Repositories.Implementations;

public class ContributorsRepository(ContributorsDbContext contributorsDbContext) : IContributorsRepository
{
    public async Task<ICollection<UserProjectEntity>> GetByProjectId(int projectId)
    {
        return await contributorsDbContext.UserProjects
            .Where(x=>x.ProjectId == projectId)
            .ToListAsync();
    }
}