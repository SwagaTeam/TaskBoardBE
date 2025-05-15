using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Mapper;

namespace ProjectService.BusinessLayer.Implementations;

public class ItemTypeManager(IItemTypeRepository itemTypeRepository) : IItemTypeManager
{
    public async Task<IEnumerable<ItemTypeModel>> GetAllAsync()
    {
        return (await itemTypeRepository.GetAllAsync())
            .Select(ItemTypeMapper.ToModel);
    }

    public async Task<ItemTypeModel> GetByIdAsync(int id)
    {
        return ItemTypeMapper.ToModel(await itemTypeRepository.GetByIdAsync(id));
    }

    public async Task<int?> CreateAsync(ItemTypeModel itemTypeModel)
    {
        var entity = ItemTypeMapper.ToEntity(itemTypeModel);
        if (entity is null) throw new NullReferenceException("Нельзя создать пустую модель");

        await itemTypeRepository.CreateAsync(entity);
        return itemTypeModel.Id;
    }

    public async Task<int?> UpdateAsync(ItemTypeModel itemTypeModel)
    {
        var entity = ItemTypeMapper.ToEntity(itemTypeModel);
        if (entity is null) throw new NullReferenceException("Нельзя создать пустую модель");
        await itemTypeRepository.UpdateAsync(entity);
        return itemTypeModel.Id;
    }

    public async Task Delete(int id)
    {
        await itemTypeRepository.DeleteAsync(id);
    }
}