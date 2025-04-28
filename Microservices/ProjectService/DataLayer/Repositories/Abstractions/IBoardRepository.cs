using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;

namespace ProjectService.DataLayer.Repositories.Abstractions
{
    public interface IBoardRepository
    {
        Task<BoardEntity?> GetById(int id);
        Task<BoardEntity?> GetByName(string name);
        Task Create(BoardEntity board);
        Task Update(BoardEntity board);
        Task Delete(int id);
        Task<IQueryable<BoardEntity>> GetByProjectId(int projectId);
    }
}
