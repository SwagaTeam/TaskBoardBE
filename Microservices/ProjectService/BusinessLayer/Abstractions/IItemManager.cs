using ProjectService.Models;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.BusinessLayer.Abstractions;

public interface IItemManager
{
    public Task<int> CreateAsync(CreateItemModel createItemModel, CancellationToken token);
    public Task<IEnumerable<ItemModel>> GetAllItemsAsync();
    public Task<ItemModel> GetByIdAsync(int id);
    public Task<ICollection<ItemModel>> GetByBoardIdAsync(int boardId);
    public Task<int> UpdateAsync(ItemModel item);
    public Task<ItemModel> GetByTitle(string title);
    public Task Delete(int id);
    public Task<ItemModel> ChangeParam(ItemModel itemModel);
    public Task<int> AddUserToItem(int userId, int itemId);
    public Task<ICollection<ItemModel>> GetItemsByUserId(int userId);

}