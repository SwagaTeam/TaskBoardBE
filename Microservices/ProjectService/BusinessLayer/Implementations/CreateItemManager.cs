using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Models;
using ProjectService.Validator;
using SharedLibrary.Auth;

namespace ProjectService.BusinessLayer.Implementations;

public class CreateItemManager(IProjectManager projectManager, IBoardManager boardManager, IAuth authManager) 
    : ICreateItemManager
{
    public async Task Validate(CreateItemModel createItemModel)
    {
        var validator = new CreateItemValidator(boardManager, projectManager, authManager);
        var result = await validator.ValidateAsync(createItemModel);
        if (!result.IsValid)
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
    }
}