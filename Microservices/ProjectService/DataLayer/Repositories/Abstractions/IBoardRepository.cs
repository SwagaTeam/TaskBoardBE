using SharedLibrary.ProjectModels;

namespace ProjectService.DataLayer.Repositories.Abstractions
{
    public interface IBoardRepository
    {
        Task<BoardModel?> GetById(int id);
        Task<BoardModel?> GetByName(string name);
        Task<int> Create(BoardModel board);
        Task<int> Update(BoardModel board);
        Task<int> Delete(int id);
    }
}
