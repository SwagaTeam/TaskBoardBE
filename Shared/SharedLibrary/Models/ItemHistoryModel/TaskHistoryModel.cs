namespace SharedLibrary.Models;
public class TaskHistoryModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ItemId { get; set; }
    public string FieldName { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public DateTime ChangedAt { get; set; }

    public string UserName { get; private set; } = "";

    public void SetUserName(string username)
    {
        UserName = username;
    }
}