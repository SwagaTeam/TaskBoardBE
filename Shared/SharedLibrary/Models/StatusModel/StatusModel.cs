

using System.Text.Json.Serialization;
using SharedLibrary.ProjectModels;

public class StatusModel
{
    public int?Id { get; set; }
    public string Name { get; set; }
    public int BoardId { get; set; }
    public int Order { get; set; }
    public bool IsDone { get; set; }
    public bool IsRejected { get; set; }
    
    [JsonIgnore]
    public ICollection<ItemModel>? Items { get; set; }
    [JsonIgnore]
    public ICollection<BoardModel>? Boards { get; set; }
}