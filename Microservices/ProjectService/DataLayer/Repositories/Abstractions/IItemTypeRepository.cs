using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Abstractions;

public interface IItemTypeRepository
{
    public Task<ItemTypeEntity> GetByIdAsync(int statusId);
    public Task<IEnumerable<ItemTypeEntity>> GetAllAsync();
    public Task CreateAsync(ItemTypeEntity statusEntity);
    public Task DeleteAsync(int id);
    public Task UpdateAsync(ItemTypeEntity statusEntity);
}