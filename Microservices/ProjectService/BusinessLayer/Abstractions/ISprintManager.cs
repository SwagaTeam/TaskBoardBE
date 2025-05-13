using ProjectService.Models;

namespace ProjectService.BusinessLayer.Abstractions
{
    public interface ISprintManager
    {
        public Task<IEnumerable<SprintModel>> GetByBoardIdAsync(int boardId);
        public Task<SprintModel> GetByIdAsync(int id);
        public Task<int?> CreateAsync(SprintModel sprintModel);
        public Task<int?> UpdateAsync(SprintModel sprintModel);
        public Task AddItem(int sprintId, int itemId);
        public Task DeleteAsync(int id);
    }
}
