using ProjectService.Models;
using SharedLibrary.Models;

namespace ProjectService.BusinessLayer.Abstractions;

public interface IValidateItemManager
{
    public Task ValidateCreateAsync(CreateItemModel createItemModel);
    public Task ValidateItemModelAsync(ItemModel itemModel);
    public Task ValidateAddUserToItemAsync(int? projectId, int? newUserId, int? itemId = null);
    public Task ValidateUserInProjectAsync(int? projectId); 
}