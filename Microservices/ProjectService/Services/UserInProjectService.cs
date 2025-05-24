using ProjectService.BusinessLayer.Abstractions;
using SharedLibrary.Auth;

namespace ProjectService.Services.MailService;

public class UserInProjectService 
{
    public static async Task<bool> IsUserInProjectAsync(IUserProjectManager userProjectManager, int? userId,
        int? projectId, CancellationToken cancellation)
    {
        if (projectId == null || projectId == -1 || userId == null || userId == -1)
            return false;

        return await userProjectManager.IsUserInProjectAsync(userId.Value, projectId.Value);
    }
    
    public static async Task<bool> IsUserCanViewAsync(IUserProjectManager userProjectManager, int? userId, 
        int? projectId, CancellationToken cancellation)
    {
        if (userId is null) return false;
        if (projectId is null) return false;
        
        return await userProjectManager.IsUserCanViewAsync((int)userId, (int)projectId);
    }

    public static async Task<bool> IsUserAdmin(IUserProjectManager userProjectManager, int? userId, int? projectId, 
        CancellationToken cancellation)
    {
        if (userId is null || userId == -1 || projectId is null || projectId == -1) return false;
        return await userProjectManager.IsUserAdminAsync((int)userId, (int)projectId);
    }
}