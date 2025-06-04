using AnalyticsService.BusinessLayer.Abstractions;
using AnalyticsService.DataLayer.Abstractions;
using AnalyticsService.Models;
using Microsoft.AspNetCore.Http;
using SharedLibrary.Constants;
using SharedLibrary.ProjectModels;
using System.Net.Http.Headers;

namespace AnalyticsService.BusinessLayer.Implementations
{
    public class ProjectManager(HttpClient httpClient, ITaskHistoryRepository taskHistoryRepository) : IProjectManager
    {
        public async Task<BurndownChartModel> GetBurndown(BurnDownChartRequest request)
        {
            var items = await httpClient.GetFromJsonAsync<IEnumerable<ItemModel>>($"item/get-items-by/{request.ProjectId}");
            var project = await httpClient.GetFromJsonAsync<ProjectModel>($"project/get/{request.ProjectId}");

            if (items is null || project is null)
                throw new InvalidOperationException("Project service вернул пустой ответ.");

            var sprintStart = project.StartDate.Date;
            var sprintEnd = project.ExpectedEndDate.Date;

            var result = new BurndownChartModel
            {
                TasksCount = items.Count(),
                StartDate = sprintStart,
                EndDate = sprintEnd,
                TasksCountByDate = new Dictionary<DateTime, int>()
            };

            for (var date = sprintStart; date <= sprintEnd; date = date.AddDays(1))
            {
                var remainingTasks = items
                    .Where(item =>
                        item.StartDate.Date <= date &&
                        item.ExpectedEndDate.Date >= date &&
                        item.Status?.IsDone == false);

                if (request.Priority <= Priority.CRITICAL && request.Priority >= Priority.VERY_LOW)
                    remainingTasks = remainingTasks.Where(item => item.Priority == request.Priority);

                result.TasksCountByDate[date] = remainingTasks.Count();
            }

            return result;
        }

    }
}
