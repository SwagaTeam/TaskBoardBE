using Kafka.Messaging.Services.Abstractions;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Mapper;
using ProjectService.Models;
using ProjectService.Validator;
using SharedLibrary.Auth;

namespace ProjectService.BusinessLayer.Implementations;

public class ItemManager(IItemRepository itemRepository, ICreateItemValidator createItemValidator,
    IKafkaProducer<ItemModel> kafkaProducer) : IItemManager
{
    public async Task<int> CreateAsync(CreateItemModel createItemModel, CancellationToken token)
    {
        await createItemValidator.CheckValidAsync(createItemModel, token);
        var item = createItemModel.Item;
        var entity = ItemMapper.ItemToEntity(item);
        await itemRepository.CreateAsync(entity);
        await kafkaProducer.ProduceAsync(item, token);
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

    public async Task<int> UpdateAsync(ItemModel item)
    {
        var entity = ItemMapper.ItemToEntity(item);
        await itemRepository.UpdateAsync(entity);
        return entity.Id;
    }

    public async Task<ItemModel> GetByTitle(string title)
    {
        return ItemMapper.ItemToModel(await itemRepository.GetByNameAsync(title));
    }
}