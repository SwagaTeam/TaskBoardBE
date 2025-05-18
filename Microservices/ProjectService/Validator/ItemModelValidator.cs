using FluentValidation;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Models;
using ProjectService.Services.MailService;
using SharedLibrary.Auth;
using SharedLibrary.Constants;

namespace ProjectService.Validator;

public class ItemModelValidator : AbstractValidator<ItemModel>
{
    private readonly IStatusManager statusManager;
    private readonly IItemTypeManager itemTypeManager;
    private readonly IItemRepository itemRepository;


    public ItemModelValidator(IStatusManager statusManager, IItemTypeManager itemTypeManager, 
        IUserProjectManager userProjectManager, IItemRepository itemRepository, int?  userId)
    {
        this.statusManager = statusManager;
        this.itemTypeManager = itemTypeManager;
        this.itemRepository = itemRepository;
        
        RuleFor(x => x)
            .MustAsync(IsStatusExist)
            .WithMessage("Такого статуса не существует");
        
        RuleFor(x => x)
            .MustAsync(IsItemTypeExist)
            .WithMessage("Такого item type не существует");
        
        RuleFor(x => x)
            .MustAsync((model, cancellation) =>
                UserInProjectService.IsUserCanViewAsync(userProjectManager, userId, model.ProjectId, cancellation))
            .WithMessage("Текущий пользователь не находится в нужном проекте");
        
        RuleFor(x => x)
            .MustAsync(IsEpicAndParentExist)
            .WithMessage("У эпика не может быть родительского item");
    }
    
    private async Task<bool> IsEpicAndParentExist(ItemModel itemModel, CancellationToken cancellation)
    {
        if (itemModel.ParentId is null) return true;
        var parent = await itemRepository.GetByIdAsync((int)itemModel.ParentId);
        return !(itemModel.ItemTypeId == ItemType.EPIC && parent is not null);
    }
    
    private async Task<bool> IsStatusExist(ItemModel item, CancellationToken cancellation)
    {
        var statusId = item.StatusId;
        if (statusId is null) return false;
        var status = await statusManager.GetByIdAsync((int)statusId);
        return status is not null;
    }
    
    private async Task<bool> IsItemTypeExist(ItemModel item, CancellationToken cancellation)
    {
        var itemTypeId = item.ItemTypeId;
        if (itemTypeId is null) return false;
        var itemType = await itemTypeManager.GetByIdAsync((int)itemTypeId);
        return itemType is not null;
    }
}