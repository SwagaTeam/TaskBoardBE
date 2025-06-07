using SharedLibrary.Models;

namespace ProjectService.BusinessLayer.Abstractions;

public interface IItemTypeManager
{
    public Task<IEnumerable<ItemTypeModel>> GetAllAsync();
    public Task<ItemTypeModel> GetByIdAsync(int id);
    public Task<int?> CreateAsync(ItemTypeModel itemTypeModel);
    public Task<int?> UpdateAsync(ItemTypeModel itemTypeModel);
    public Task Delete(int id);
}