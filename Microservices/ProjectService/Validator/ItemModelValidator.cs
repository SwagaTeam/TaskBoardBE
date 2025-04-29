using FluentValidation;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Models;
using SharedLibrary.Auth;

namespace ProjectService.Validator;

public class ItemModelValidator : AbstractValidator<ItemModel>
{
    private readonly IStatusManager statusManager;
    private readonly IItemTypeManager itemTypeManager;

    public ItemModelValidator(IStatusManager statusManager, IItemTypeManager itemTypeManager)
    {
        this.statusManager = statusManager;
        this.itemTypeManager = itemTypeManager;
        
        RuleFor(x => x)
            .MustAsync(IsStatusExist)
            .WithMessage("Такого статуса не существует");
        
        RuleFor(x => x)
            .MustAsync(IsItemTypeExist)
            .WithMessage("Такого item type не существует");
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