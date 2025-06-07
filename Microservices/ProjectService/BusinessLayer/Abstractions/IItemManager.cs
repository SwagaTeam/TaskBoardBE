using ProjectService.Models;
using SharedLibrary.Constants;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.Models;

namespace ProjectService.BusinessLayer.Abstractions;

public interface IItemManager
{
    public Task<int> CreateAsync(CreateItemModel createItemModel, CancellationToken token);
    public Task<IEnumerable<ItemModel>> GetAllItemsAsync();
    public Task<ItemModel> GetByIdAsync(int? id);
    public Task<ICollection<ItemModel>> GetByBoardIdAsync(int boardId);
    public Task<ICollection<ItemModel>> GetItemsByUserId(int userId, int projectId);
    public Task<ICollection<ItemModel>> GetCurrentUserItems();

    public Task<int> UpdateAsync(ItemModel item, CancellationToken token, string message, string oldValue, string newValue,
        string fieldName,
        TaskEventType eventType = TaskEventType.Updated);
    public Task<ItemModel> GetByTitle(string title);
    public Task Delete(int id);
    public Task<ICollection<ItemModel>> GetByProjectIdAsync(int projectId);
    public Task<int> AddUserToItemAsync(int newUserId, int itemId, CancellationToken cancellationToken);
    public Task<int> AddCommentToItemAsync(CommentModel commentModel, IFormFile? attachment);
    public Task<ICollection<CommentModel>> GetComments(int itemId);
    public Task<ICollection<ItemModel>> GetArchievedItemsInProject(int projectId);
    public Task<ICollection<ItemModel>> GetArchievedItemsInBoard(int boardId);
}