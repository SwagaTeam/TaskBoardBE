using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer.Repositories.Abstractions;
using SharedLibrary.Constants;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Implementations;

public class UserProjectRepository(ProjectDbContext context) : IUserProjectRepository
{
    public async Task CreateAsync(UserProjectEntity userProject)
    {
        var project = await context.Projects
            .Include(p => p.UserProjects)
            .FirstOrDefaultAsync(p => p.Id == userProject.ProjectId);

        if (project == null)
            throw new InvalidOperationException("Project not found");

        project.UserProjects.Add(userProject);

        await context.SaveChangesAsync();
    }
    
    public async Task<bool> IsUserAdminAsync(int userId, int projectId)
    {
        var userProject = await context.UserProjects
            .FirstOrDefaultAsync(x => x.ProjectId == projectId 
                                      && x.UserId == userId 
                                      && x.Privilege == Privilege.ADMIN);

        return userProject is not null;
    }

    public async Task<bool> IsUserCanViewAsync(int userId, int projectId)
    {
        var userProject = await context.UserProjects
            .Include(x=>x.Project)
            .FirstOrDefaultAsync(x => x.ProjectId == projectId
                                      && x.UserId == userId
                                      && (Enumerable.Range(0, 3).Contains(x.Privilege)
                                          || !x.Project.IsPrivate));

        return userProject is not null;
    }

    public async Task<bool> IsUserInProjectAsync(int userId, int projectId)
    {
        var userProject = await context.UserProjects
            .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.UserId == userId);

        return userProject is not null;
    }

    public async Task<bool> IsUserViewerAsync(int userId, int projectId)
    {
        var userInProject = await IsUserInProjectAsync(userId, projectId);
        if (!userInProject) return true;
        var userIsViewer = await context.UserProjects
            .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.UserId == userId 
                                                               && x.Privilege == Privilege.VIEWER);
        return userIsViewer is not null;
    }
}