namespace ProjectService.BusinessLayer.Abstractions;

public interface IValidateDocumentManager
{
    public Task ValidateUserInProjectAsync(int? projectId);

}