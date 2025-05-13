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
    private readonly IProjectManager projectManager;
    private readonly IBoardManager boardManager;
    private readonly IAuth authManager;
    private readonly IItemRepository itemRepository;
    
    public CreateItemValidator(IBoardManager boardManager, IProjectManager projectManager, IAuth authManager, 
        IItemRepository itemRepository, IStatusManager statusManager, IItemTypeManager itemTypeManager)
    {
        this.boardManager = boardManager;
        this.projectManager = projectManager;
        this.authManager = authManager;
        this.itemRepository = itemRepository;
        
        RuleFor(x => x.Item)
            .SetValidator(new ItemModelValidator(statusManager, itemTypeManager));
        
        RuleFor(x => x)
            .MustAsync(IsUserMember)
            .WithMessage("Пользователь не имеет прав на создание задач");

        RuleFor(x => x)
            .MustAsync(BeValidBoard)
            .WithMessage("Неверный boardId");

        RuleFor(x => x)
            .MustAsync(IsEpicAndParentExist)
            .WithMessage("У эпика не может быть родительского item");
    }

    private async Task<bool> IsUserMember(CreateItemModel item, CancellationToken cancellation)
    {
        var currentId = authManager.GetCurrentUserId();
        if (currentId is null) return false;
        
        return !await projectManager.IsUserViewerAsync((int)currentId, (int)item.Item.ProjectId);
    }

    //у эпика не может быть родителя
    private async Task<bool> IsEpicAndParentExist(CreateItemModel createItemModel, CancellationToken cancellation)
    {
        var item = createItemModel.Item;
        if (item.ParentId is null) return true;
        var parent = await itemRepository.GetByIdAsync((int)item.ParentId);
        return !(item.ItemTypeId == ItemType.EPIC && parent is not null);
    }
    
    private async Task<bool> BeValidBoard(CreateItemModel item, CancellationToken cancellation)
    {
        var board = await boardManager.GetByIdAsync(item.BoardId);
        return board != null && board.ProjectId == item.Item.ProjectId;
    }
}