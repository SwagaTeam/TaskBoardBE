using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
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
                .Include(x => x.UserProjects)
                .Include(x => x.Boards)
                .FirstOrDefaultAsync(x => x.Boards.Any(x => x.Id == id));

            return project;
        }

        public IQueryable<ProjectEntity?> GetByUserId(int? currentUserId)
        {
            var projects = projectDbContext.Projects
                .Include(x => x.UserProjects)
                .Where(x => x.UserProjects.Any(x => x.UserId == currentUserId));

            return projects;
        }

        public async Task<ProjectEntity?> GetByIdAsync(int id)
        {
            var project = await projectDbContext.Projects
                .Include(x => x.UserProjects)
                .FirstOrDefaultAsync(x => x.Id == id);

            return project;
        }


        public async Task Update(ProjectEntity project)
        {
            var existing = await GetByIdAsync(project.Id);
            if (existing is null) throw new ProjectNotFoundException();

            existing.Id = project.Id;
            existing.Name = project.Name;
            existing.Key = project.Key;
            existing.Description = project.Description;
            existing.IsPrivate = project.IsPrivate;
            existing.CreatedAt = project.CreatedAt;
            existing.UpdatedAt = project.UpdatedAt;
            existing.ExpectedEndDate = project.ExpectedEndDate;
            existing.Priority = project.Priority;

            await projectDbContext.SaveChangesAsync();
        }

        public async Task<int> SetUserRoleAsync(int userId, int projectId, RoleEntity role)
        {
            var userProject = await projectDbContext.UserProjects.FirstOrDefaultAsync(x => x.ProjectId == projectId
                && x.UserId == userId);

            var existingRole = await projectDbContext.Roles.FirstOrDefaultAsync(x => x.Role == role.Role);

            if (userProject is not null)
            {
                if (existingRole is not null)
                    userProject.Role = existingRole;
                else
                    userProject.Role = role;
            }
            else return -1;

            await projectDbContext.SaveChangesAsync();

            return userId;
        }
    }
}