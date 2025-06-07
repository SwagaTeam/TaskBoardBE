using SharedLibrary.Entities.AnalyticsService;

namespace AnalyticsService.DataLayer.Abstractions;

public interface ITaskHistoryRepository
{
    public Task<IEnumerable<TaskHistoryEntity>> GetHistoryByTaskIdAsync(int taskId);
    public Task<IEnumerable<TaskHistoryEntity>> GetHistoryByManyTaskIds(HashSet<int> taskIds);
    public Task CreateAsync(TaskHistoryEntity entity);

}