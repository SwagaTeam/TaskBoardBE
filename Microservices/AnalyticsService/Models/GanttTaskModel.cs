namespace AnalyticsService.Models
{
    public class GanttTaskModel
    {
        public string Id { get; set; } = string.Empty;
        public string? Parent { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Start { get; set; } = string.Empty; // формат "yyyy-MM-dd"
        public string End { get; set; } = string.Empty;
        public string? Status { get; set; }
        public List<string?> Assignee { get; set; }
    }

}
