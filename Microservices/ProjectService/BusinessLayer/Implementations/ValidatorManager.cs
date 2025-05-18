using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Models;
using ProjectService.Validator;
using SharedLibrary.Auth;

namespace ProjectService.BusinessLayer.Implementations;

public class ValidatorManager(
    IProjectManager projectManager,
    IBoardManager boardManager,
    IAuth authManager,
    IItemRepository itemRepository,
    IStatusManager statusManager,
    IItemTypeManager itemTypeManager,
    IUserProjectManager userProjectManager)
    : IValidatorManager
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

    public async Task ValidateAddUserToItemAsync(int? projectId, int newUserId)
    {
        var userId = authManager.GetCurrentUserId();
        var validateModel = new UsersInProjectModel
        {
            CurrentUserId = userId,
            NewUserId = newUserId,
            ProjectId = projectId
        };

        var validator = new AddUserToItemValidator(userProjectManager);
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

    public async Task ValidateUserAdminAsync(int? projectId)
    {
        var userId = authManager.GetCurrentUserId();
        var validator = new UserAdminValidator(userProjectManager, userId, projectId);
        var result = await validator.ValidateAsync("");
        if (!result.IsValid)
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
    }
    
    public async Task ValidateUserCanViewAsync(int? projectId, int? userId = null)
    {
        var validator = new UserMemberValidator(userProjectManager, userId ?? authManager.GetCurrentUserId(), projectId);
        var result = await validator.ValidateAsync("");
        if (!result.IsValid)
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
    }
}