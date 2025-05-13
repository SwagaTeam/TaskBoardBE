using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Abstractions
{
    public interface ISprintRepository
    {
        public Task<SprintEntity> GetByIdAsync(int sprintId);
        public Task<IEnumerable<SprintEntity>> GetAllAsync();
        public Task<IEnumerable<SprintEntity>> GetByBoardId(int boardId);
        public Task CreateAsync(SprintEntity sprintEntity);
        public Task DeleteAsync(int id);
        public Task UpdateAsync(SprintEntity sprintEntity);
        public Task AddItem(int sprintId, int itemId);
    }
}
