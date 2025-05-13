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
            .Select(ItemMapper.ItemToModel);
        return items;
    }

    public async Task<ItemModel> GetByIdAsync(int id)
    {
        var model = ItemMapper.ItemToModel(await itemRepository.GetByIdAsync(id));
        return model;
    }

    public async Task<ICollection<ItemModel>> GetByBoardIdAsync(int boardId)
    {
        var items = await itemRepository.GetByBoardIdAsync(boardId);
        return items.Select(ItemMapper.ItemToModel).ToList();
    }

    public async Task<int> UpdateAsync(ItemModel item)
    {
        var entity = ItemMapper.ItemToEntity(item);
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
        await itemRepository.AddUserToItem(itemUserEntity);
        return itemUserEntity.Id;
    }

    public async Task<ItemModel> GetByTitle(string title)
    {
        return ItemMapper.ItemToModel(await itemRepository.GetByNameAsync(title));
    }

    public async Task<ItemModel> ChangeParam(ItemModel itemModel)
    {
        await validateItemManager.ValidateItemModelAsync(itemModel);
        await UpdateAsync(itemModel);
        return itemModel;
    }

    public async Task<ICollection<ItemModel>> GetItemsByUserId(int userId)
    {
        //TODO: Добавить валидацию
        var items = await itemRepository.GetItemsByUserId(userId);
        return items.Select(ItemMapper.ItemToModel).ToList();
    }

    public async Task UpdateStatus(UpdateItemStatusModel model)
    {
        var item = await itemRepository.GetByIdAsync(model.ItemId);

        if (item is null)
            throw new ItemNotFoundException();

        var status = await statusRepository.GetByIdAsync(model.StatusId);

        if (status is null)
            throw new StatusNotFoundException();

        item.Status = status;

        await itemRepository.UpdateStatusAsync(item);
    }
}