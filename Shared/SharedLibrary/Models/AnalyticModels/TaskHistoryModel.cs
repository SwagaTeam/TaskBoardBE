namespace SharedLibrary.Models.AnalyticModels;

public class TaskHistoryModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ItemId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public string OldValue { get; set; } = string.Empty;
    public string NewValue { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; }
}