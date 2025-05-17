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
    
    public static async Task<bool> IsUserMember(IUserProjectManager userProjectManager, IAuth authManager, 
        int? projectId, CancellationToken cancellation)
    {
        var currentId = authManager.GetCurrentUserId();
        if (currentId is null) return false;
        if (projectId is null) return false;
        
        return !await userProjectManager.IsUserViewerAsync((int)currentId, (int)projectId);
    }
}