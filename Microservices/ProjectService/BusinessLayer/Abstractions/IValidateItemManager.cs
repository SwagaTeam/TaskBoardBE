using ProjectService.Models;

namespace ProjectService.BusinessLayer.Abstractions;

public interface IValidateItemManager
{
    public Task ValidateCreateAsync(CreateItemModel createItemModel);
    public Task ValidateItemModelAsync(ItemModel itemModel);
    public Task ValidateAddUserToItemAsync(int? projectId, int newUserId);
    public Task ValidateUserInProjectAsync(int? projectId);


}