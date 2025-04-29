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
    private readonly IStatusManager statusManager;
    private readonly IItemTypeManager itemTypeManager;
    
    public CreateItemValidator(IBoardManager boardManager, IProjectManager projectManager, IAuth authManager, 
        IItemRepository itemRepository, IStatusManager statusManager, IItemTypeManager itemTypeManager)
    {
        this.boardManager = boardManager;
        this.projectManager = projectManager;
        this.authManager = authManager;
        this.itemRepository = itemRepository;
        this.statusManager = statusManager;
        this.itemTypeManager = itemTypeManager;
        
        RuleFor(x => x)
            .MustAsync(IsUserMember)
            .WithMessage("Пользователь не имеет прав на создание задач");

        RuleFor(x => x)
            .MustAsync(BeValidBoard)
            .WithMessage("Неверный boardId");

        RuleFor(x => x)
            .MustAsync(IsEpicAndParentExist)
            .WithMessage("У эпика не может быть родительского item");

        RuleFor(x => x)
            .MustAsync(IsStatusExist)
            .WithMessage("Такого статуса не существует");
        
        RuleFor(x => x)
            .MustAsync(IsItemTypeExist)
            .WithMessage("Такого item type не существует");
    }

    private async Task<bool> IsUserMember(CreateItemModel item, CancellationToken cancellation)
    {
        var currentId = authManager.GetCurrentUserId();
        if (currentId is null) return false;
        
        return !await projectManager.IsUserViewer((int)currentId, (int)item.Item.ProjectId);
    }

    private async Task<bool> IsStatusExist(CreateItemModel item, CancellationToken cancellation)
    {
        var statusId = item.Item.StatusId;
        if (statusId is null) return false;
        var status = await statusManager.GetById((int)statusId);
        return status is not null;
    }
    
    private async Task<bool> IsItemTypeExist(CreateItemModel item, CancellationToken cancellation)
    {
        var itemTypeId = item.Item.ItemTypeId;
        if (itemTypeId is null) return false;
        var itemType = await itemTypeManager.GetById((int)itemTypeId);
        return itemType is not null;
    }

    private async Task<bool> IsEpicAndParentExist(CreateItemModel createItemModel, CancellationToken cancellation)
    {
        var item = createItemModel.Item;
        if (item.ParentId is null) return false;
        var parent = await itemRepository.GetByIdAsync((int)item.ParentId);
        return item.ItemTypeId == ItemType.EPIC && parent is not null;
    }
    
    private async Task<bool> BeValidBoard(CreateItemModel item, CancellationToken cancellation)
    {
        var board = await boardManager.GetById(item.BoardId);
        return board != null && board.ProjectId == item.Item.ProjectId;
    }
}