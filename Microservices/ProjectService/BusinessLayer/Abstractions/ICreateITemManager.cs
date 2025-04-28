using ProjectService.Models;

namespace ProjectService.BusinessLayer.Abstractions;

public interface ICreateItemManager
{
    public Task Validate(CreateItemModel createItemModel);
}