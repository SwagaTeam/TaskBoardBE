using SharedLibrary.Constants;

namespace SharedLibrary.Models.KafkaModel;

public class TaskEventMessage
{
    public TaskEventType EventType { get; set; } 
    public ICollection<UserItemModel> UserItems { get; set; } = new List<UserItemModel>();
    public string Message { get; set; } = string.Empty;
}