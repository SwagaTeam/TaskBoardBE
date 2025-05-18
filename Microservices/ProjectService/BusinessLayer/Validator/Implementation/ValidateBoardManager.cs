using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Validator;
using SharedLibrary.Auth;

namespace ProjectService.BusinessLayer.Implementations;

public class ValidateBoardManager(IAuth authManager, IUserProjectManager userProjectManager) : IValidateBoardManager
{
    public async Task ValidateUserAdminAsync(int? projectId)
    {
        var userId = authManager.GetCurrentUserId();
        var validator = new UserAdminValidator(userProjectManager, userId, projectId);
        var result = await validator.ValidateAsync("");
        if (!result.IsValid)
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
    }
    
    public async Task ValidateUserCanViewAsync(int? projectId, int? userId = null)
    {
        var validator = new UserMemberValidator(userProjectManager, userId ?? authManager.GetCurrentUserId(), projectId);
        var result = await validator.ValidateAsync("");
        if (!result.IsValid)
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
    }
}