using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Abstractions;

public interface IStatusRepository
{
    public Task<StatusEntity> GetByIdAsync(int statusId);
    public Task<IEnumerable<StatusEntity>> GetAllAsync();
    public Task CreateAsync(StatusEntity statusEntity);
    public Task DeleteAsync(int id);
    public Task UpdateAsync(StatusEntity statusEntity);
    public Task<IQueryable<StatusEntity>> GetByBoardIdAsync(int boardId);
    public Task UpdateRangeAsync(ICollection<StatusEntity> statusEntities);
}