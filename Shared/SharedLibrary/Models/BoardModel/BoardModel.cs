using System.Text.Json.Serialization;
using SharedLibrary.ProjectModels;

public class BoardModel
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int StatusId { get; set; }

    [JsonIgnore]
    public ProjectModel? Project { get; set; }

    public ICollection<StatusModel>? Statuses { get; set; }

    [JsonIgnore]
    public ICollection<SprintModel>? Sprints { get; set; } = new List<SprintModel>();
}