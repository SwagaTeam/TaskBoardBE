using AnalyticsService.Models;
using SharedLibrary.Models;

namespace AnalyticsService.BusinessLayer.Abstractions
{
    public interface IProjectManager
    {
        public Task<BurndownChartModel> GetBurndown(BurnDownChartRequest request);
        public Task<ICollection<ChartDataPoint>> GetCustomChart(ChartQueryModel query);
        public Task<ICollection<GanttTaskModel>> GetGanttChartDataAsync(int projectId);
        public Task<List<HeatmapCell>> GetHeatmapData(HeatmapQueryModel query);
        public Task<ICollection<TaskHistoryModel>> GetProjectHistory(int projectId);
        public Task<ICollection<RoadmapItemModel>> GetRoadmapDataAsync(int projectId);
        public Task<IEnumerable<StatusCountModel>> GetCumulativeFlow(int projectId, DateTime startDate, DateTime endDate);
        public Task<IEnumerable<StatusCountModel>> GetFunnelChartAsync(int projectId);

    }
}
