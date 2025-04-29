using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;

namespace ProjectService.DataLayer.Repositories.Abstractions
{
    public interface IBoardRepository
    {
        Task<BoardEntity?> GetByIdAsync(int id);
        Task<BoardEntity?> GetByNameAsync(string name);
        Task CreateAsync(BoardEntity board);
        Task UpdateAsync(BoardEntity board);
        Task DeleteAsync(int id);
        Task<IQueryable<BoardEntity>> GetByProjectIdAsync(int projectId);
        Task UpdateRangeAsync(ICollection<BoardEntity> boards);
    }
}
