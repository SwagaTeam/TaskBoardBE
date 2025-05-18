using FluentValidation;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Models;
using SharedLibrary.Auth;
using SharedLibrary.Constants;

namespace ProjectService.Validator;

public class CreateItemValidator
    : AbstractValidator<CreateItemModel>
{
    private readonly IBoardManager boardManager;
    
    public CreateItemValidator(IBoardManager boardManager, IItemRepository itemRepository, IStatusManager statusManager, 
        IItemTypeManager itemTypeManager, IUserProjectManager userProjectManager, int? userId)
    {
        this.boardManager = boardManager;
        
        RuleFor(x => x.Item)
            .SetValidator(new ItemModelValidator(statusManager, itemTypeManager, userProjectManager, itemRepository, userId));

        RuleFor(x => x)
            .MustAsync(BeValidBoard)
            .WithMessage("Неверный boardId");
    }
    
    private async Task<bool> BeValidBoard(CreateItemModel item, CancellationToken cancellation)
    {
        var board = await boardManager.GetByIdAsync(item.BoardId);
        return board != null && board.ProjectId == item.Item.ProjectId;
    }
}