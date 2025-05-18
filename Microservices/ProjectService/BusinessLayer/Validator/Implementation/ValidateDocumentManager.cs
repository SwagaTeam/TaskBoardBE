using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Validator;
using SharedLibrary.Auth;

namespace ProjectService.BusinessLayer.Implementations;

public class ValidateDocumentManager(IAuth authManager, IUserProjectManager userProjectManager) : IValidateDocumentManager
{
    public async Task ValidateUserInProjectAsync(int? projectId)
    {
        var userId = authManager.GetCurrentUserId();
        var validator = new UserInProjectValidator(userProjectManager, userId, projectId);
        var result = await validator.ValidateAsync("");
        if (!result.IsValid)
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
    }
}