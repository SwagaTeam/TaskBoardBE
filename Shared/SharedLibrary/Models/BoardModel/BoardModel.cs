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
    public ProjectModel? Project { get; set; } = null;

    [JsonIgnore]
    public StatusModel? Status { get; set; } = null;

    [JsonIgnore]
    public ICollection<SprintModel>? Sprints { get; set; } = null;
}