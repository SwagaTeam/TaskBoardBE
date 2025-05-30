using SharedLibrary.Models.AnalyticModels;

namespace AnalyticsService.BusinessLayer.Implementations;

public interface ITaskManager
{
    public Task<int> GetCompletedTaskCountBetween(int projectId, DateTime startDate, DateTime endDate);
    public Task<IEnumerable<ItemModel>> GetCompletedTaskBetween(int projectId, DateTime startDate, DateTime endDate);
    public Task<IDictionary<string, TimeSpan>> GetAverageTimeInStatusesAsync(int taskId);
    public Task<TimeSpan> GetTotalTimeOutsideStatusAsync(int taskId, string excludedStatus);
    public Task<TimeSpan?> GetAverageTimeInStatusAsync(int taskId, string statusName);
    public Task<int> CreateAsync(TaskHistoryModel model);
}