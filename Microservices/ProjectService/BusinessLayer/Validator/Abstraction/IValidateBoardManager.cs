namespace ProjectService.BusinessLayer.Abstractions;

public interface IValidateBoardManager
{
    public Task ValidateUserAdminAsync(int? projectId);
    public Task ValidateUserCanViewAsync(int? projectId, int? userId = null);

}