using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer.Repositories.Abstractions;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Implementations;

public class ContributorsRepository(ProjectDbContext context) : IContributorsRepository
{
    public async Task<ICollection<UserProjectEntity>> GetByProjectId(int projectId)
    {
        return await context.UserProjects
            .Include(x=>x.Project)
            .Include(x=>x.Role)
            .Where(x=>x.ProjectId == projectId)
            .ToListAsync();
    }
}