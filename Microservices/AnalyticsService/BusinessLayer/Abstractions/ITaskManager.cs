namespace AnalyticsService.BusinessLayer.Implementations;

public interface ITaskManager
{
    public Task<int> GetCompletedTaskCountBetween(int projectId, DateTime startDate, DateTime endDate);
    public Task<IEnumerable<ItemModel>> GetCompletedTaskBetween(int projectId, DateTime startDate, DateTime endDate);
    public Task<IDictionary<string, TimeSpan>> CalculateAvgTimeInStatus(int taskId);
}