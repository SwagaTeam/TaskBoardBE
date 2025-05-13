using Kafka.Messaging.Services.Abstractions;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using ProjectService.Mapper;
using ProjectService.Models;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.BusinessLayer.Implementations;

public class ItemManager(
    IItemRepository itemRepository, 
    IValidateItemManager validateItemManager,
    IKafkaProducer<ItemModel> kafkaProducer, 
    IItemBoardsRepository itemBoardsRepository, 
    IStatusRepository statusRepository, 
    IProjectRepository projectRepository) : IItemManager
{
    public async Task<int> CreateAsync(CreateItemModel createItemModel, CancellationToken token)
    {
        await validateItemManager.ValidateCreateAsync(createItemModel);

        var project = await projectRepository.GetByBoardIdAsync(createItemModel.BoardId);

        var item = createItemModel.Item;
        var entity = ItemMapper.ItemToEntity(item);
        entity.CreatedAt = DateTime.UtcNow;

        await itemRepository.CreateAsync(entity);

        entity.BusinessId = $"{project.Key}-ITEM-{entity.Id}";

        await itemBoardsRepository.Create(
            new ItemBoardEntity()
            {
                ItemId = entity.Id,
                BoardId = createItemModel.BoardId,
                StatusId = createItemModel.StatusId
            }
        );

        return entity.Id;
    }

    public async Task Delete(int id)
    {
        await itemRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ItemModel>> GetAllItemsAsync()
    {
        var items = (await itemRepository.GetItemsAsync())
            .Select(ItemMapper.ToModel);
        return items;
    }

    public async Task<ItemModel> GetByIdAsync(int id)
    {
        var model = ItemMapper.ToModel(await itemRepository.GetByIdAsync(id));
        return model;
    }

    public async Task<ICollection<ItemModel>> GetByBoardIdAsync(int boardId)
    {
        var items = await itemRepository.GetItemsByBoardIdAsync(boardId);
        return items.Select(ItemMapper.ToModel).ToList();
    }

    public async Task<int> UpdateAsync(ItemModel item)
    {
        await validateItemManager.ValidateItemModelAsync(item);
        var entity = ItemMapper.ItemToEntity(item);
        entity.UpdatedAt = DateTime.UtcNow;
        await itemRepository.UpdateAsync(entity);
        return entity.Id;
    }
    
    public async Task<int> AddUserToItem(int userId, int itemId)
    {
        //TODO: Добавить валидацию (Текущий юзер в проекте, добавляемый юзер в проекте, item в проекте текущего юзера)
        var itemUserEntity = new UserItemEntity()
        {
            ItemId = itemId,
            UserId = userId
        };
        await itemRepository.AddUserToItemAsync(itemUserEntity);
        return itemUserEntity.Id;
    }

    public async Task<ItemModel> GetByTitle(string title)
    {
        return ItemMapper.ToModel(await itemRepository.GetByNameAsync(title));
    }

    public async Task<ICollection<ItemModel>> GetItemsByUserId(int userId)
    {
        //TODO: Добавить валидацию
        var items = await itemRepository.GetItemsByUserIdAsync(userId);
        return items.Select(ItemMapper.ToModel).ToList();
    }

    public async Task<ICollection<ItemModel>> GetArchievedItemsInProject(int projectId)
    {
        //TODO: Добавить валидацию

        var items = await itemRepository.GetItemsByProjectIdAsync(projectId);

        return items.Where(x => x.IsArchived).Select(ItemMapper.ToModel).ToList();
    }

    public async Task<ICollection<ItemModel>> GetArchievedItemsInBoard(int boardId)
    {
        //TODO: Добавить валидацию

        var items = await itemRepository.GetItemsByBoardIdAsync(boardId);

        return items.Where(x => x.IsArchived).Select(ItemMapper.ToModel).ToList();
    }
}