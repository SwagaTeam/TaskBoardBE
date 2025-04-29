namespace ProjectService.BusinessLayer.Abstractions;

public interface IStatusManager
{
    public Task<IEnumerable<StatusModel>> GetAll();
    public Task<StatusModel> GetById(int id);
    public Task<int?> Create(StatusModel statusModel);
    public Task<int?> Update(StatusModel statusModel);
    public Task Delete(int id);
}