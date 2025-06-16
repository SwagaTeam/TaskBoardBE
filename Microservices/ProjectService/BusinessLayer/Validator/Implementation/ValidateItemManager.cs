using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Models;
using ProjectService.Validator;
using SharedLibrary.Auth;
using SharedLibrary.Models;

namespace ProjectService.BusinessLayer.Implementations;

public class ValidateItemManager(IBoardManager boardManager,
    IAuth authManager,
    IItemRepository itemRepository,
    IStatusManager statusManager,
    IItemTypeManager itemTypeManager,
    IUserProjectManager userProjectManager) : IValidateItemManager
{
    public async Task ValidateCreateAsync(CreateItemModel createItemModel)
    {
        var userId = authManager.GetCurrentUserId();
        var validator = new CreateItemValidator(boardManager, itemRepository, statusManager, itemTypeManager, userProjectManager, userId);
        var result = await validator.ValidateAsync(createItemModel);
        if (!result.IsValid)
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
    }
    
    public async Task ValidateItemModelAsync(ItemModel itemModel)
    {
        var userId = authManager.GetCurrentUserId();
        var validator = new ItemModelValidator(statusManager, itemTypeManager, userProjectManager, itemRepository, userId);
        var result = await validator.ValidateAsync(itemModel);
        if (!result.IsValid)
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
    }
    

    public async Task ValidateAddUserToItemAsync(int? projectId, int? newUserId, int? itemId = null)
    {
        var userId = authManager.GetCurrentUserId();
        var validateModel = new UsersInProjectModel
        {
            CurrentUserId = userId,
            NewUserId = newUserId,
            ProjectId = projectId,
            ItemId = itemId
        };

        var validator = new AddUserToItemValidator(userProjectManager, itemRepository);
        var result = await validator.ValidateAsync(validateModel);
        if (!result.IsValid)
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
    }
    
    public async Task ValidateUserInProjectAsync(int? projectId)
    {
        var userId = authManager.GetCurrentUserId();
        var validator = new UserInProjectValidator(userProjectManager, userId, projectId);
        var result = await validator.ValidateAsync("");
        if (!result.IsValid)
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
    }
}