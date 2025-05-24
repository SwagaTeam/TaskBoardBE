using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Mapper;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;

namespace ProjectService.DataLayer.Repositories.Implementations
{
    public class ProjectLinkRepository(ProjectDbContext context) : IProjectLinkRepository
    {
        public async Task CreateAsync(ProjectLinkEntity projectLink)
        {
            await context.VisibilityLinks.AddAsync(projectLink);
            await context.SaveChangesAsync();
        }

        public async Task<ProjectLinkEntity?> GetByIdAsync(int id)
        {
            var projectLink = await context.VisibilityLinks
                .Include(x => x.Project)
                .ThenInclude(x=>x.UserProjects)
                .FirstOrDefaultAsync(x => x.Id == id);
            return projectLink;
        }

        public async Task<ProjectLinkEntity?> GetByLinkAsync(string link)
        {
            var projectLink = await context.VisibilityLinks
                .Include(x => x.Project)
                .ThenInclude(x=>x.UserProjects)
                .FirstOrDefaultAsync(x => x.Url == link);
            return projectLink;
        }
    }
}
