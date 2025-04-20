using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Mapper;
using SharedLibrary.Constants;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;

namespace ProjectService.DataLayer.Repositories.Implementations
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectDbContext _projectDbContext;
        public ProjectRepository(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<int> AddUserInProject(int userId, int projectId)
        {
            var result = await _projectDbContext.UserProjects.AddAsync(new UserProjectEntity()
            {
                ProjectId = projectId,
                UserId = userId,
                Role = Roles.MEMBER
            });

            await _projectDbContext.SaveChangesAsync();

            return projectId;
        }

        public async Task<int> Create(ProjectModel project, int userId)
        {
            var result = await _projectDbContext.Projects.AddAsync(ProjectMapper.ProjectModelToProjectEntity(project));

            await _projectDbContext.SaveChangesAsync();

            await _projectDbContext.UserProjects.AddAsync(new UserProjectEntity()
            {
                ProjectId = result.Entity.Id,
                UserId = userId,
                Role = Roles.ADMIN
            });

            await _projectDbContext.SaveChangesAsync();

            return project.Id;
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectModel?> GetById(int id)
        {
            var project = await _projectDbContext.Projects.FindAsync(id);

            if (project is null)
                return null;

            return ProjectMapper.ProjectEntityToProjectModel(project);
        }

        public async Task<bool> IsUserInProject(int userId, int projectId)
        {
            var userProject = await _projectDbContext.UserProjects.FirstOrDefaultAsync(x => x.ProjectId == projectId && x.UserId == userId);

            return userProject is not null;
        }

        public Task<int> Update(ProjectModel project)
        {
            throw new NotImplementedException();
        }
    }
}
