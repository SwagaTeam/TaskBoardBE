namespace AnalyticsService.Models
{
    public class BurndownChartModel
    {
        public int TasksCount { get; set; }
        public Dictionary<DateTime, int> TasksCountByDate {  get; set; } = new Dictionary<DateTime, int>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
