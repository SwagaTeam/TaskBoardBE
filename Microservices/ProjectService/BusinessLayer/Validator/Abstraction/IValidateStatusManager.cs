namespace ProjectService.BusinessLayer.Abstractions;

public interface IValidateStatusManager
{
    public Task ValidateUserInProjectAsync(int? projectId);
    public Task ValidateUserAdminAsync(int? projectId);
}