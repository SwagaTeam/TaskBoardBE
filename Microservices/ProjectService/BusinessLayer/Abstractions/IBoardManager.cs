using SharedLibrary.ProjectModels;

namespace ProjectService.BusinessLayer.Abstractions
{
    public interface IBoardManager
    {
        Task<BoardModel?> GetById(int id);
        Task<int> Create(BoardModel board);
        Task<int> Update(BoardModel board);
        Task<int> Delete(int id);
    }
}
