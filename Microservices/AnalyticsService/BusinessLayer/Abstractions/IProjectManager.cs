using AnalyticsService.Models;
using SharedLibrary.Models;

namespace AnalyticsService.BusinessLayer.Abstractions
{
    public interface IProjectManager
    {
        public Task<BurndownChartModel> GetBurndown(BurnDownChartRequest request);
        public Task<ICollection<TaskHistoryModel>> GetProjectHistory(int projectId);
    }
}
