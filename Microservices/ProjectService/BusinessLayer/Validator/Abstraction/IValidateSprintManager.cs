namespace ProjectService.BusinessLayer.Abstractions;

public interface IValidateSprintManager
{
    public Task ValidateUserInProjectAsync(int? projectId);

}