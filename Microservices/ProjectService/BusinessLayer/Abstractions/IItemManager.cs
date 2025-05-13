using ProjectService.Models;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.BusinessLayer.Abstractions;

public interface IItemManager
{
    public Task<int> CreateAsync(CreateItemModel createItemModel, CancellationToken token);
    public Task<IEnumerable<ItemModel>> GetAllItemsAsync();
    public Task<ItemModel> GetByIdAsync(int? id);
    public Task<ICollection<ItemModel>> GetByBoardIdAsync(int boardId);
    public Task<ICollection<ItemModel>> GetItemsByUserId(int userId, int projectId);
    public Task<ICollection<ItemModel>> GetCurrentUserItems();


    public Task<int> UpdateAsync(ItemModel item);
    public Task<ItemModel> GetByTitle(string title);
    public Task Delete(int id);
    public Task<int> AddUserToItemAsync(int userId, int itemId);
    public Task<ICollection<ItemModel>> GetArchievedItemsInProject(int projectId);
    public Task<ICollection<ItemModel>> GetArchievedItemsInBoard(int board);
}