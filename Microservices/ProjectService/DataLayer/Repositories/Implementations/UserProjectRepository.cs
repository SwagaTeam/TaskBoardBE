using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer.Repositories.Abstractions;
using SharedLibrary.Constants;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Implementations;

public class UserProjectRepository(ProjectDbContext context) : IUserProjectRepository
{
    public async Task CreateAsync(UserProjectEntity userProject)
    {
        await context.UserProjects.AddAsync(userProject);
        await context.SaveChangesAsync();
    }
    
    public async Task<bool> IsUserAdmin(int userId, int projectId)
    {
        var userProject = await context.UserProjects
            .FirstOrDefaultAsync(x => x.ProjectId == projectId 
                                      && x.UserId == userId 
                                      && x.Privilege == Privilege.ADMIN);

        return userProject is not null;
    }

    public async Task<bool> IsUserCanView(int userId, int projectId)
    {
        var userProject = await context.UserProjects
            .Include(x=>x.Project)
            .FirstOrDefaultAsync(x => x.ProjectId == projectId
                                      && x.UserId == userId
                                      && (Enumerable.Range(0, 3).Contains(x.Privilege)
                                          || x.Project.IsPrivate == false));

        return userProject is not null;
    }

    public async Task<bool> IsUserInProject(int userId, int projectId)
    {
        var userProject = await context.UserProjects
            .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.UserId == userId);

        return userProject is not null;
    }

    public async Task<bool> IsUserViewer(int userId, int projectId)
    {
        var userInProject = await IsUserInProject(userId, projectId);
        if (!userInProject) return true;
        var userIsViewer = await context.UserProjects
            .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.UserId == userId 
                                                               && x.Privilege == Privilege.VIEWER);
        return userIsViewer is not null;
    }
}