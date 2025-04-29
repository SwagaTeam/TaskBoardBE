using SharedLibrary.ProjectModels;

namespace ProjectService.BusinessLayer.Abstractions
{
    public interface IBoardManager
    {
        Task<BoardModel?> GetByIdAsync(int id);
        Task<ICollection<BoardModel>> GetByProjectIdAsync(int projectId);
        Task ChangeBoardOrderAsync(int boardId, int newOrder);
        Task<int> CreateAsync(BoardModel board);
        Task<int> UpdateAsync(BoardModel board);
        Task<int> DeleteAsync(int id);
    }
}
