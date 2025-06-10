namespace AnalyticsService.Models
{
    public class ChartQueryModel
    {
        public int ProjectId { get; set; }
        public string XAxis { get; set; }
        public string YAxis { get; set; } = "count";
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
