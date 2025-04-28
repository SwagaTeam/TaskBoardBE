using ProjectService.Models;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.BusinessLayer.Abstractions;

public interface IItemManager
{
    public Task<int> CreateAsync(CreateItemModel createItemModel, CancellationToken token);
    public Task<IEnumerable<ItemModel>> GetAllItemsAsync();
    public Task<ItemModel> GetByIdAsync(int id);
    public Task<int> UpdateAsync(ItemModel item);
    public Task<ItemModel> GetByTitle(string title);
    public Task Delete(int id); }