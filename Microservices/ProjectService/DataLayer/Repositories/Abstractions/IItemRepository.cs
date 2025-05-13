using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Abstractions;

public interface IItemRepository
{
    public Task<ItemEntity> GetByIdAsync(int id);
    public Task CreateAsync(ItemEntity item);
    public Task UpdateAsync(ItemEntity item);
    public Task UpdateStatusAsync(ItemEntity item);
    public Task DeleteAsync(int id);
    public Task<ICollection<ItemEntity>> GetItemsAsync();
    public Task<ItemEntity> GetByNameAsync(string name);
    public Task<ICollection<ItemEntity>> GetByBoardIdAsync(int boardId);
    public Task<ICollection<ItemEntity>> GetItemsByUserId(int userId);
    public Task AddUserToItem(UserItemEntity itemUserEntity);
}