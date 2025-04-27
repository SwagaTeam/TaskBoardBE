using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Mapper;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;

namespace ProjectService.DataLayer.Repositories.Implementations
{
    public class ProjectLinkRepository : IProjectLinkRepository
    {
        private readonly ProjectDbContext _context;
        public ProjectLinkRepository(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(ProjectLinkModel projectLink)
        {
            await _context.VisibilityLinks.AddAsync(new VisibilityLinkEntity()
            {
                ProjectId = projectLink.ProjectId,
                Url = projectLink.URL
            });

            await _context.SaveChangesAsync();

            return projectLink.ProjectId;
        }

        public async Task<ProjectLinkModel?> Get(int id)
        {
            var projectLink = await _context.VisibilityLinks.FindAsync(id);
            if (projectLink == null)
                return null;

            return new ProjectLinkModel() {ProjectId = projectLink.ProjectId, URL = projectLink.Url};
        }

        public async Task<ProjectLinkModel?> GetByLink(string link)
        {
            var projectLink = await _context.VisibilityLinks.FirstOrDefaultAsync(x=>x.Url == link);

            if (projectLink == null)
                return null;

            var project = await _context.Projects.FindAsync(projectLink.ProjectId);
            


            return new ProjectLinkModel() { ProjectId = projectLink.ProjectId, URL = projectLink.Url , Project = ProjectMapper.ToModel(project)};
        }
    }
}
