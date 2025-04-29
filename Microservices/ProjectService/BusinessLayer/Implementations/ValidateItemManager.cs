using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Models;
using ProjectService.Validator;
using SharedLibrary.Auth;

namespace ProjectService.BusinessLayer.Implementations;

public class ValidateItemManager(IProjectManager projectManager, IBoardManager boardManager, IAuth authManager, 
    IItemRepository itemRepository, IStatusManager statusManager, IItemTypeManager itemTypeManager) 
    : IValidateItemManager
{
    public async Task ValidateCreateAsync(CreateItemModel createItemModel)
    {
        var validator = new CreateItemValidator(boardManager, projectManager, authManager, itemRepository, statusManager, itemTypeManager);
        var result = await validator.ValidateAsync(createItemModel);
        if (!result.IsValid)
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
    }

    public async Task ValidateItemModelAsync(ItemModel itemModel)
    {
        var validator = new ItemModelValidator(statusManager, itemTypeManager);
        var result = await validator.ValidateAsync(itemModel);
        if (!result.IsValid)
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
    }
}