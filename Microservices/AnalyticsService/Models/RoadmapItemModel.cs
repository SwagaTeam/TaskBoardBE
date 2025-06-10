namespace AnalyticsService.Models
{
    public class RoadmapItemModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string? Status { get; set; }
        public List<string> Assignees { get; set; } = new();
    }

}
