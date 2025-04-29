namespace ProjectService.BusinessLayer.Abstractions;

public interface IItemTypeManager
{
    public Task<IEnumerable<ItemTypeModel>> GetAll();
    public Task<ItemTypeModel> GetById(int id);
    public Task<int?> Create(ItemTypeModel statusModel);
    public Task<int?> Update(ItemTypeModel statusModel);
    public Task Delete(int id);
}