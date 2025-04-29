using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Mapper;
using SharedLibrary.Constants;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;

namespace ProjectService.DataLayer.Repositories.Implementations
{
    public class ProjectRepository(ProjectDbContext projectDbContext) : IProjectRepository
    {

        public async Task Create(ProjectEntity projectEntity)
        {
            await projectDbContext.Projects.AddAsync(projectEntity);
            await projectDbContext.SaveChangesAsync();
        }


        public async Task Delete(int id)
        {
            var project = await projectDbContext.Projects.FindAsync(id);

            if (project != null)
            {
                projectDbContext.Projects.Remove(project);
                await projectDbContext.SaveChangesAsync();
            }
        }
        

        public async Task<ProjectEntity?> GetByBoardIdAsync(int id)
        {
            var project = await projectDbContext.Projects
                .Include(x => x.Boards)
                .FirstOrDefaultAsync(x => x.Boards.Any(x=>x.Id == id));

            return project;
        }

        public async Task<ProjectEntity?> GetByIdAsync(int id)
        {
            var project = await projectDbContext.Projects
                .Include(x=>x.UserProjects)
                .FirstOrDefaultAsync(x=>x.Id == id);

            return project;
        }
        

        public async Task Update(ProjectEntity project)
        {
            projectDbContext.Projects.Update(project);
            await projectDbContext.SaveChangesAsync();
        }
    }
}
