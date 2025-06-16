using AnalyticsService.BusinessLayer.Abstractions;
using AnalyticsService.DataLayer.Abstractions;
using AnalyticsService.Mapper;
using AnalyticsService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using SharedLibrary.Constants;
using SharedLibrary.Dapper.DapperRepositories.Abstractions;
using SharedLibrary.Models;
using SharedLibrary.ProjectModels;
using System.Net.Http.Headers;

namespace AnalyticsService.BusinessLayer.Implementations
{
    public class ProjectManager(
        HttpClient httpClient,
        ITaskHistoryRepository taskHistoryRepository,
        IUserRepository userRepository) : IProjectManager
    {
        public async Task<BurndownChartModel> GetBurndown(BurnDownChartRequest request)
        {
            var items =
                await httpClient.GetFromJsonAsync<IEnumerable<ItemModel>>($"item/get-items-by/{request.ProjectId}");
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

        public async Task<ICollection<ChartDataPoint>> GetCustomChart(ChartQueryModel query)
        {
            var items = await httpClient.GetFromJsonAsync<IEnumerable<ItemModel>>(
                $"item/get-items-by/{query.ProjectId}");

            items = items
                .Where(i => i.StartDate >= query.Start && i.StartDate <= query.End)
                .ToList();

            var yAxis = query.YAxis?.ToLower() ?? "count";

            List<ChartDataPoint> data;

            switch (query.XAxis.ToLower())
            {
                case "status":
                    data = items
                        .Where(i => i.Status != null)
                        .GroupBy(i => i.Status!.Name)
                        .Select(g => new ChartDataPoint
                        {
                            X = g.Key,
                            Y = CalculateYAxis(g, yAxis)
                        })
                        .ToList();
                    break;

                case "user":
                    var allContributors = items
                        .SelectMany(i => i.Contributors)
                        .Distinct()
                        .ToList();

                    if (items.Any(i => i.Contributors.Count == 0))
                    {
                        allContributors.Add("Без исполнителя");
                    }

                    data = allContributors
                        .Select(contributor => new ChartDataPoint
                        {
                            X = contributor,
                            Y = CalculateYAxis(
                                items.Where(i =>
                                    (contributor == "Без исполнителя" && i.Contributors.Count == 0) ||
                                    (contributor != "Без исполнителя" && i.Contributors.Contains(contributor))
                                ),
                                yAxis)
                        })
                        .ToList();
                    break;

                case "date":
                    data = items
                        .GroupBy(i => i.StartDate.Date)
                        .Select(g => new ChartDataPoint
                        {
                            X = g.Key.ToString("yyyy-MM-dd"),
                            Y = CalculateYAxis(g, yAxis)
                        })
                        .ToList();
                    break;

                default:
                    throw new ArgumentException($"Unsupported XAxis: {query.XAxis}");
            }

            return data;
        }

        private static double CalculateYAxis(IEnumerable<ItemModel> group, string yAxis)
        {
            return yAxis switch
            {
                "count" => group.Count(),
                "sum-priority" => group.Sum(i => i.Priority),
                "avg-duration" => group
                    .Select(i => (i.ExpectedEndDate - i.StartDate).TotalDays)
                    .DefaultIfEmpty(0)
                    .Average(),
                _ => throw new ArgumentException($"Unsupported YAxis: {yAxis}")
            };
        }
        
        public async Task<IEnumerable<StatusCountModel>> GetFunnelChartAsync(int projectId)
        {
            var items = await httpClient.GetFromJsonAsync<IEnumerable<ItemModel>>($"item/get-items-by/{projectId}");

            var statuses = (await httpClient.GetFromJsonAsync<List<StatusModel>>("statuses/get-all")).Select(x=>x.Name);

            var grouped = items
                .Where(i => i.Status != null)
                .GroupBy(i => i.Status!.Name)
                .ToDictionary(g => g.Key, g => g.Count());

            var result = statuses
                .Where(status => grouped.ContainsKey(status))
                .Select(status => new StatusCountModel
                {
                    Status = status,
                    Count = grouped[status]
                })
                .ToList();

            return result;
        }


        public async Task<IEnumerable<StatusCountModel>> GetCumulativeFlow(int projectId, DateTime startDate,
            DateTime endDate)
        {
            var items = await httpClient.GetFromJsonAsync<IEnumerable<ItemModel>>($"item/get-items-by/{projectId}");
            var itemIds = items.Select(i => i.Id).ToHashSet();

            var itemHistory = await taskHistoryRepository.GetHistoryByManyTaskIds(itemIds);

            var historyByItem = itemHistory
                .Where(h => h.FieldName == "Status")
                .GroupBy(h => h.ItemId)
                .ToDictionary(g => g.Key, g => g.OrderBy(h => h.ChangedAt).ToList());

            var dailyStatusMap = new Dictionary<DateTime, Dictionary<string, int>>();

            foreach (var item in items)
            {
                var history = historyByItem.GetValueOrDefault(item.Id, []);
                var statusTimeline = new List<(DateTime Date, string Status)>();

                if (history.Count == 0)
                {
                    if (!string.IsNullOrWhiteSpace(item.Status?.Name))
                        statusTimeline.Add((startDate, item.Status.Name));
                }
                else
                {
                    statusTimeline.AddRange(history.Select(h => (h.ChangedAt.Date, h.NewValue)));

                    if (!string.IsNullOrWhiteSpace(item.Status?.Name) &&
                        statusTimeline.Last().Date < endDate)
                    {
                        statusTimeline.Add((endDate, item.Status.Name));
                    }

                    if (statusTimeline.First().Date > startDate &&
                        !string.IsNullOrWhiteSpace(history.First().OldValue))
                    {
                        statusTimeline.Insert(0, (startDate, history.First().OldValue));
                    }
                }

                for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                {
                    var currentStatus = GetStatusOnDate(statusTimeline, date);
                    if (currentStatus == null)
                        continue;

                    if (!dailyStatusMap.ContainsKey(date))
                        dailyStatusMap[date] = new Dictionary<string, int>();

                    if (!dailyStatusMap[date].ContainsKey(currentStatus))
                        dailyStatusMap[date][currentStatus] = 0;

                    dailyStatusMap[date][currentStatus]++;
                }
            }

            var result = new List<StatusCountModel>();
            foreach (var date in dailyStatusMap.Keys.OrderBy(d => d))
            {
                foreach (var (status, count) in dailyStatusMap[date])
                {
                    result.Add(new StatusCountModel
                    {
                        Status = $"{date:yyyy-MM-dd}|{status}",
                        Count = count
                    });
                }
            }

            return result;
        }
        
        private static string? GetStatusOnDate(List<(DateTime Date, string Status)> timeline, DateTime targetDate)
        {
            var filtered = timeline.Where(t => t.Date <= targetDate).ToList();
            return filtered.LastOrDefault().Status;
        }


        public async Task<ICollection<TaskHistoryModel>> GetProjectHistory(int projectId)
        {
            var items = await httpClient.GetFromJsonAsync<IEnumerable<ItemModel>>($"item/get-items-by/{projectId}");

            var itemIds = items.Select(x => x.Id).ToHashSet();

            var itemHistory = await taskHistoryRepository.GetHistoryByManyTaskIds(itemIds);

            var historyModels = await Task.WhenAll(
                itemHistory.Select(i => TaskHistoryMapper.ToModel(i, userRepository)));

            return historyModels.OrderByDescending(x => x.ChangedAt).ToList();
        }

        public async Task<ICollection<GanttTaskModel>> GetGanttChartDataAsync(int projectId)
        {
            var items = await httpClient.GetFromJsonAsync<IEnumerable<ItemModel>>($"item/get-items-by/{projectId}");

            var ganttTasks = items
                .Where(i => i.StartDate != default && i.ExpectedEndDate != default)
                .Select(i => new GanttTaskModel
                {
                    Id = i.Id.ToString(),
                    Name = i.Title,
                    Start = i.StartDate.ToString("yyyy-MM-dd"),
                    End = i.ExpectedEndDate.ToString("yyyy-MM-dd"),
                    Parent = i.ParentId?.ToString(),
                    Status = i.Status?.Name,
                    Assignee = i.Contributors
                })
                .ToList();

            return ganttTasks;
        }

        public async Task<ICollection<RoadmapItemModel>> GetRoadmapDataAsync(int projectId)
        {
            var items = await httpClient.GetFromJsonAsync<IEnumerable<ItemModel>>($"item/get-items-by/{projectId}");

            var roadmapItems = items
                .Where(i => i.StartDate != default && i.ExpectedEndDate != default)
                .Select(i => new RoadmapItemModel
                {
                    Id = i.Id,
                    Title = i.Title,
                    Start = i.StartDate,
                    End = i.ExpectedEndDate,
                    Status = i.Status?.Name,
                    Assignees = i.Contributors
                })
                .ToList();

            return roadmapItems;
        }

        /// <summary>
        /// Получение данных для построения тепловой карты (heatmap)
        /// </summary>
        /// <remarks>
        /// <b>Описание метрик:</b>
        /// <ul>
        ///     <li><b>inflow</b> – количество задач, созданных в указанный день (по дате начала)</li>
        ///     <li><b>outflow</b> – количество завершённых задач в указанный день (по дате обновления и статусу "Завершено")</li>
        ///     <li><b>avg-duration</b> – средняя длительность задач (в днях) от начала до ожидаемого завершения в конкретный день</li>
        /// </ul>
        /// <b>Оси:</b>
        /// <ul>
        ///     <li><b>X</b> – дата (в формате YYYY-MM-DD)</li>
        ///     <li><b>Y</b> – исполнитель задачи (если не назначен – "Без исполнителя")</li>
        ///     <li><b>Value</b> – значение метрики (количество задач или средняя длительность)</li>
        /// </ul>
        /// </remarks>
        /// <param name="query">Параметры фильтрации: идентификатор проекта, диапазон дат, тип метрики</param>
        /// <returns>Список ячеек тепловой карты с координатами и значением</returns>
        public async Task<List<HeatmapCell>> GetHeatmapData(HeatmapQueryModel query)
        {
            var items = await httpClient.GetFromJsonAsync<IEnumerable<ItemModel>>(
                $"item/get-items-by/{query.ProjectId}");

            var filteredItems = items
                .Where(i => i.StartDate.Date >= query.Start.Date && i.StartDate.Date <= query.End.Date)
                .ToList();

            return query.Metric.ToLower() switch
            {
                "inflow" => filteredItems
                    .SelectMany(i => i.Contributors.DefaultIfEmpty("Без исполнителя"),
                        (item, contributor) => new { contributor, date = item.StartDate.Date })
                    .GroupBy(g => new { g.date, g.contributor })
                    .Select(g => new HeatmapCell
                    {
                        X = g.Key.date.ToString("yyyy-MM-dd"),
                        Y = g.Key.contributor,
                        Value = g.Count()
                    }).ToList(),

                "outflow" => filteredItems
                    .Where(i => i.Status?.Name == "Завершено") // При необходимости заменить на актуальный статус
                    .SelectMany(i => i.Contributors.DefaultIfEmpty("Без исполнителя"),
                        (item, contributor) => new { contributor, date = item.UpdatedAt.Date })
                    .GroupBy(g => new { g.date, g.contributor })
                    .Select(g => new HeatmapCell
                    {
                        X = g.Key.date.ToString("yyyy-MM-dd"),
                        Y = g.Key.contributor,
                        Value = g.Count()
                    }).ToList(),

                "avg-duration" => filteredItems
                    .SelectMany(i => i.Contributors.DefaultIfEmpty("Без исполнителя"),
                        (item, contributor) => new
                        {
                            contributor,
                            date = item.StartDate.Date,
                            duration = (item.ExpectedEndDate - item.StartDate).TotalDays
                        })
                    .GroupBy(g => new { g.date, g.contributor })
                    .Select(g => new HeatmapCell
                    {
                        X = g.Key.date.ToString("yyyy-MM-dd"),
                        Y = g.Key.contributor,
                        Value = Math.Round(g.Average(x => x.duration), 2)
                    }).ToList(),

                _ => throw new ArgumentException("Unknown metric: " + query.Metric)
            };
        }
    }
}