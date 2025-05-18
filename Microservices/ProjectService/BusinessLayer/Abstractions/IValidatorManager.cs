using ProjectService.Models;

namespace ProjectService.BusinessLayer.Abstractions;

public interface IValidatorManager
{
    public Task ValidateCreateAsync(CreateItemModel createItemModel);
    public Task ValidateItemModelAsync(ItemModel itemModel);
    public Task ValidateAddUserToItemAsync(int? projectId, int newUserId);
    public Task ValidateUserInProjectAsync(int? projectId);
    public Task ValidateUserAdminAsync(int? projectId);
    public Task ValidateUserCanViewAsync(int? projectId, int? userId = null);

}