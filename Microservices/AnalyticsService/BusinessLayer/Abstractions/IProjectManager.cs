using AnalyticsService.Models;

namespace AnalyticsService.BusinessLayer.Abstractions
{
    public interface IProjectManager
    {
        public Task<BurndownChartModel> GetBurndown(BurnDownChartRequest request);
    }
}
