namespace AnalyticsService.Models
{
    public class HeatmapQueryModel
    {
        public int ProjectId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        /// <summary> Метрика: inflow, outflow, avg-duration </summary>
        public string Metric { get; set; } = "inflow";
    }
}
