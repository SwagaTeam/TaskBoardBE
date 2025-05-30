using AnalyticsService.DataLayer.Abstractions;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Entities.AnalyticsService;

namespace AnalyticsService.DataLayer.Implementations;

public class TaskHistoryRepository(AnalyticsDbContext context) : ITaskHistoryRepository
{
    public async Task<IEnumerable<TaskHistoryEntity>> GetHistoryByTaskIdAsync(int taskId)
    {
        return await context.TaskHistories
            .Where(x => x.ItemId == taskId)
            .ToListAsync();
    }

}