using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Abstractions;

public interface IItemTypeRepository
{
    public Task<ItemTypeEntity> GetByIdAsync(int itemTypeId);
    public Task<IEnumerable<ItemTypeEntity>> GetAllAsync();
    public Task CreateAsync(ItemTypeEntity itemTypeEntity);
    public Task DeleteAsync(int id);
    public Task UpdateAsync(ItemTypeEntity itemTypeEntity);
}