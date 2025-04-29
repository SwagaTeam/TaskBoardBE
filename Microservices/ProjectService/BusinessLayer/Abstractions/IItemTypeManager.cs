namespace ProjectService.BusinessLayer.Abstractions;

public interface IItemTypeManager
{
    public Task<IEnumerable<ItemTypeModel>> GetAllAsync();
    public Task<ItemTypeModel> GetByIdAsync(int id);
    public Task<int?> CreateAsync(ItemTypeModel statusModel);
    public Task<int?> UpdateAsync(ItemTypeModel statusModel);
    public Task Delete(int id);
}