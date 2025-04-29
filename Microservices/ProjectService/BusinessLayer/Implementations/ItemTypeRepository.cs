using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Mapper;

namespace ProjectService.BusinessLayer.Implementations;

public class ItemTypeRepository(IItemTypeRepository itemTypeRepository) : IItemTypeManager
{
    public async Task<IEnumerable<ItemTypeModel>> GetAll()
    {
        return (await itemTypeRepository.GetAllAsync())
            .Select(ItemTypeMapper.ToModel);
    }

    public async Task<ItemTypeModel> GetById(int id)
    {
        return ItemTypeMapper.ToModel(await itemTypeRepository.GetByIdAsync(id));
    }

    public async Task<int?> Create(ItemTypeModel itemTypeModel)
    {
        var entity = ItemTypeMapper.ToEntity(itemTypeModel);
        if (entity is null) throw new NullReferenceException("Нельзя создать пустую модель");
        
        await itemTypeRepository.CreateAsync(entity);
        return itemTypeModel.Id;
    }

    public async Task<int?> Update(ItemTypeModel itemTypeModel)
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