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

        public async Task Create(ProjectEntity projectEntity, int userId)
        {
            var result = await _projectDbContext.Projects.AddAsync(projectEntity);

            await _projectDbContext.SaveChangesAsync();

            await _projectDbContext.UserProjects.AddAsync(new UserProjectEntity()
            {
                ProjectId = result.Entity.Id,
                UserId = userId,
                RoleId = DefaultRoles.CREATOR,
                Privilege = Privilege.ADMIN
            }); 

            await _projectDbContext.SaveChangesAsync();
        }


        public async Task Delete(int id)
        {
            var project = await _projectDbContext.Projects.FindAsync(id);

            if (project != null)
            {
                _projectDbContext.Projects.Remove(project);
                await _projectDbContext.SaveChangesAsync();
            }
        }

        public async Task<ProjectEntity?> GetByBoardId(int id)
        {
            var project = await _projectDbContext.Projects
                .Include(x => x.Boards)
                .FirstOrDefaultAsync(x => x.Boards.Any(x=>x.Id == id));

            return project;
        }

        public async Task<ProjectEntity?> GetById(int id)
        {
            var project = await _projectDbContext.Projects
                .Include(x=>x.UserProjects)
                .FirstOrDefaultAsync(x=>x.Id == id);

            return project;
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
            var userProject = await _projectDbContext.UserProjects
                .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.UserId == userId);

            return userProject is not null;
        }

        public async Task<bool> IsUserViewer(int userId, int projectId)
        {
            var userInProject = await IsUserInProject(userId, projectId);
            if (!userInProject) return true;
            var userIsViewer = await _projectDbContext.UserProjects
                .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.UserId == userId 
                                                                   && x.Privilege == Privilege.VIEWER);
            return userIsViewer is not null;
        }

        public Task  Update(ProjectEntity project)
        {
            throw new NotImplementedException();
        }
    }
}
