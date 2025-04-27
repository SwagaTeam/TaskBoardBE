using Microsoft.AspNetCore.Authorization.Infrastructure;
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
                Privilege = Privilege.MEMBER
            });

            await _projectDbContext.SaveChangesAsync();

            return projectId;
        }

        public async Task<int> Create(ProjectModel project, int userId)
        {
            var projectEntity = ProjectMapper.ToEntity(project);

            projectEntity.CreatedAt = DateTime.UtcNow;

            var result = await _projectDbContext.Projects.AddAsync(projectEntity);

            await _projectDbContext.SaveChangesAsync();

            await _projectDbContext.UserProjects.AddAsync(new UserProjectEntity()
            {
                ProjectId = result.Entity.Id,
                UserId = userId,
                Role = new RoleEntity() {Role = "Создатель"},
                Privilege = Privilege.ADMIN
            }); 

            await _projectDbContext.SaveChangesAsync();

            return project.Id;
        }

        public async Task<int> Delete(int id)
        {
            var project = await _projectDbContext.Projects.FindAsync(id);

            if (project != null)
            {
                _projectDbContext.Projects.Remove(project);
                await _projectDbContext.SaveChangesAsync();
                return id;
            }

            return -1;
        }

        public async Task<ProjectModel?> GetByBoardId(int id)
        {
            var project = await _projectDbContext.Projects
                .Include(x => x.Boards)
                .FirstOrDefaultAsync(x => x.Boards.Any(x=>x.Id == id));

            if (project is null)
                return null;

            return ProjectMapper.ToModel(project);
        }

        public async Task<ProjectModel?> GetById(int id)
        {
            var project = await _projectDbContext.Projects
                .Include(x=>x.UserProjects)
                .FirstOrDefaultAsync(x=>x.Id == id);

            if (project is null)
                return null;

            return ProjectMapper.ToModel(project);
        }

        public async Task<bool> IsUserAdmin(int userId, int projectId)
        {
            var userProject = await _projectDbContext.UserProjects
                .FirstOrDefaultAsync(x => x.ProjectId == projectId 
                                       && x.UserId == userId 
                                       && x.Privilege == Privilege.ADMIN);

            return userProject is not null;
        }

        public async Task<bool> IsUserCanView(int userId, int projectId)
        {
            var userProject = await _projectDbContext.UserProjects
                .Include(x=>x.Project)
                .FirstOrDefaultAsync(x => x.ProjectId == projectId
                                       && x.UserId == userId
                                       && (Enumerable.Range(0, 3).Contains(x.Privilege)
                                       || x.Project.IsPrivate == false));

            return userProject is not null;
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
