using AnalyticsService.BusinessLayer.Implementations;
using AnalyticsService.DataLayer.Abstractions;
using SharedLibrary.Entities.AnalyticsService;
using SharedLibrary.Models.AnalyticModels;

namespace AnalyticsService.BusinessLayer.Abstractions;

public class TaskManager(HttpClient httpClient, ITaskHistoryRepository taskHistoryRepository) : ITaskManager
{
    public async Task<int> CreateAsync(TaskHistoryModel model)
    {
        var entity = new TaskHistoryEntity
        {
            ChangedAt = model.ChangedAt,
            UserId = model.UserId,
            OldValue = model.OldValue,
            NewValue = model.NewValue,
            FieldName = model.FieldName,
            ItemId = model.ItemId
        };
        await taskHistoryRepository.CreateAsync(entity);
        return entity.Id;
    }
    
    public async Task<IEnumerable<ItemModel>> GetCompletedTaskBetween(int projectId, DateTime startDate, DateTime endDate)
    {
        var items = 
            (await httpClient.GetFromJsonAsync<IEnumerable<ItemModel>>($"get-items-by/{projectId}"))
            .Where(item=>IsTaskCompleted(item) && IsTaskBetweenDates(item, startDate, endDate));
        return items;
    }

    public async Task<int> GetCompletedTaskCountBetween(int projectId, DateTime startDate, DateTime endDate)
    {
        return (await GetCompletedTaskBetween(projectId, startDate, endDate)).Count();
    }
    
    public async Task<TimeSpan?> GetAverageTimeInStatusAsync(int taskId, string statusName)
    {
        var item = await httpClient.GetFromJsonAsync<ItemModel>($"{taskId}");
        var start = item.StartDate;

        var history = (await taskHistoryRepository.GetHistoryByTaskIdAsync(taskId))
            .Where(h => h.FieldName == "Status")
            .OrderBy(h => h.ChangedAt)
            .ToList();

        var totalTime = TimeSpan.Zero;
        var count = 0;

        foreach (var entry in history)
        {
            if (entry.OldValue == statusName)
            {
                var duration = entry.ChangedAt - start;
                totalTime += duration;
                count++;
            }

            start = entry.ChangedAt;
        }

        // Учитываем, если последний статус — это искомый
        var lastStatus = history.LastOrDefault()?.NewValue ?? "Unknown";
        if (lastStatus == statusName)
        {
            var endTime = item.ExpectedEndDate;
            var duration = endTime - start;
            totalTime += duration;
            count++;
        }

        if (count == 0)
            return null; // Никогда не был в этом статусе

        return totalTime / count;
    }
    
    public async Task<TimeSpan> GetTotalTimeOutsideStatusAsync(int taskId, string excludedStatus)
    {
        var item = await httpClient.GetFromJsonAsync<ItemModel>($"{taskId}");
        var history = (await taskHistoryRepository.GetHistoryByTaskIdAsync(taskId))
            .Where(h => h.FieldName == "StatusId")
            .OrderBy(h => h.ChangedAt)
            .ToList();

        var total = TimeSpan.Zero;
        var current = item.StartDate;
        var lastStatus = "";

        foreach (var h in history)
        {
            if (lastStatus != excludedStatus)
                total += h.ChangedAt - current;

            current = h.ChangedAt;
            lastStatus = h.NewValue;
        }

        if (lastStatus != excludedStatus)
            total += item.ExpectedEndDate - current;

        return total;
    }
    
    public async Task<IDictionary<string, TimeSpan>> GetAverageTimeInStatusesAsync(int taskId)
    {
        var item = await httpClient.GetFromJsonAsync<ItemModel>($"{taskId}");
        var start = item.StartDate;

        var timeSpent = new Dictionary<string, TimeSpan>();
        var count = new Dictionary<string, int>();

        var history = (await taskHistoryRepository.GetHistoryByTaskIdAsync(taskId))
            .Where(h => h.FieldName == "StatusId")
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