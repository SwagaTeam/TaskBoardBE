using AnalyticsService.DataLayer;
using AnalyticsService.DataLayer.Abstractions;

namespace AnalyticsService.BusinessLayer.Abstractions;

public class TaskManager(HttpClient httpClient, ITaskHistoryRepository taskHistoryRepository)
{
    public async Task<IEnumerable<ItemModel>> GetCompletedTaskBetween(int projectId, DateTime startDate, DateTime endDate)
    {
        var items = 
            (await httpClient.GetFromJsonAsync<IEnumerable<ItemModel>>($"item/get-items-by/{projectId}"))
            .Where(item=>IsTaskCompleted(item) && IsTaskBetweenDates(item, startDate, endDate));
        return items;
    }

    public async Task<int> GetCompletedTaskCountBetween(int projectId, DateTime startDate, DateTime endDate)
    {
        return (await GetCompletedTaskBetween(projectId, startDate, endDate)).Count();
    }

    public async Task<IDictionary<string, TimeSpan>> CalculateAvgTimeInStatus(int taskId)
    {
        var item = await httpClient.GetFromJsonAsync<ItemModel>($"item/{taskId}");
        var start = item.StartDate;

        var timeSpent = new Dictionary<string, TimeSpan>();
        var count = new Dictionary<string, int>();

        var history = (await taskHistoryRepository.GetHistoryByTaskIdAsync(taskId))
            .Where(h => h.FieldName == "Status")
            .OrderBy(h => h.ChangedAt)
            .ToList();

        foreach (var entry in history)
        {
            var oldStatus = entry.OldValue;

            var duration = entry.ChangedAt - start;

            if (timeSpent.TryAdd(oldStatus, duration))
            {
                count[oldStatus] = 1;
            }
            else
            {
                timeSpent[oldStatus] += duration;
                count[oldStatus]++;
            }

            start = entry.ChangedAt;
        }

        var lastStatus = history.LastOrDefault()?.NewValue ?? "Unknown";
        var endTime = item.ExpectedEndDate;
        var lastDuration = endTime - start;

        if (timeSpent.TryAdd(lastStatus, lastDuration))
        {
            count[lastStatus] = 1;
        }
        else
        {
            timeSpent[lastStatus] += lastDuration;
            count[lastStatus]++;
        }
        
        var result = new Dictionary<string, TimeSpan>();
        
        foreach (var status in timeSpent.Keys)
        {
            var average = timeSpent[status] / count[status];
            result.Add(status, average);
        }
        
        return result;
    }

    private bool IsTaskCompleted(ItemModel itemModel)
    {
        return itemModel.Status is not null && itemModel.Status.Name.Equals("Готово");
    }

    private bool IsTaskBetweenDates(ItemModel itemModel, DateTime startDate, DateTime endDate)
    {
        return itemModel.ExpectedEndDate >= startDate && itemModel.ExpectedEndDate <= endDate;
    }
}