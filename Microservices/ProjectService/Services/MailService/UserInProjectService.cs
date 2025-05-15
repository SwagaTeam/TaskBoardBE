using ProjectService.BusinessLayer.Abstractions;

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
}